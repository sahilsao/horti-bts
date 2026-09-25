using System.Data;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Notices;
using MySqlConnector;

namespace HortiBts.Api.Repositories.Notices;

public interface INoticeRepository
{
    Task<Result<List<NoticesDto>>> GetActiveAsync();
    Task<Result<int>> SaveNoticeAsync(AddNoticesDto dto, IFormFile? file, string userId, string clientIp);
    Task<Result<int>> UpdateNoticeAsync(AddNoticesDto dto, IFormFile? file, string userId, string clientIp);
    Task<Result<bool>> UpdateActiveFlagAsync(int noticeId, bool flag, string userId, string clientIp);
}

public class NoticeRepository(IDbConnectionFactory connectionFactory, IWebHostEnvironment env) : INoticeRepository
{
    public async Task<Result<List<NoticesDto>>> GetActiveAsync()
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
        SELECT
            nb.notice_id    AS NoticeId,
            nb.subject      AS Subject,
            nb.description  AS Description,
            nb.start_date   AS StartDate,
            nb.end_date     AS EndDate,
            nb.priority     AS Priority,
            nb.notice_type  AS NoticeType,
            nb.status       AS Status,
            p.file_name     AS FileName,
            p.path          AS Path
        FROM noticeboard nb
        LEFT JOIN noticeboard_file_path p ON p.notice_id = nb.notice_id";

        var notices = await connection.QueryAsync<NoticesDto>(Sql);
        return Result<List<NoticesDto>>.Success(notices.ToList());
    }

    public async Task<Result<int>> SaveNoticeAsync(AddNoticesDto dto, IFormFile? file, string userId, string clientIp)
    {
        using var connection = connectionFactory.CreateConnection(HortiDb.Bts);

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            const string insertSql = @"
                INSERT INTO noticeboard
                    (subject, description, start_date, end_date, priority,
                     status, notice_type, created_by, ip_address)
                VALUES
                    (@Subject, @Description, @StartDate, @EndDate, @Priority,
                     1, @NoticeType, @UserId, @IpAddress);
                SELECT LAST_INSERT_ID();";

            var insertedNoticeId = await connection.ExecuteScalarAsync<int>(insertSql, new
            {
                dto.Subject,
                dto.Description,
                dto.StartDate,
                dto.EndDate,
                dto.Priority,
                dto.NoticeType,
                UserId = userId,
                IpAddress = clientIp
            }, transaction);

            if (file is not null)
            {
                var savedPath = await SaveFileToDiskAsync(file, insertedNoticeId);

                const string filePathSql = @"
                    INSERT INTO noticeboard_file_path (notice_id, path, file_name, created_by, ip_address)
                    VALUES (@NoticeId, @Path, @FileName, @UserId, @IpAddress);";

                await connection.ExecuteAsync(filePathSql, new
                {
                    NoticeId = insertedNoticeId,
                    Path = savedPath.Path,
                    FileName = savedPath.FileName,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);
            }

            transaction.Commit();
            return Result<int>.Success(insertedNoticeId);
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            transaction.Rollback();
            return Result<int>.Failure("A notice with this name already exists.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Result<int>.Failure($"Unable to save notice: {ex.Message}");
        }
    }

    public async Task<Result<int>> UpdateNoticeAsync(AddNoticesDto dto, IFormFile? file, string userId, string clientIp)
    {
        using var connection = connectionFactory.CreateConnection(HortiDb.Bts);

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            const string updateSql = @"
                UPDATE noticeboard
                SET 
                    subject = @Subject,
                    description = @Description,
                    start_date = @StartDate,
                    end_date = @EndDate,
                    priority = @Priority,
                    notice_type = @NoticeType,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE notice_id = @NoticeId;";

            var rows = await connection.ExecuteAsync(updateSql, new
            {
                dto.NoticeId,
                dto.NoticeType,
                dto.Subject,
                dto.Description,
                dto.StartDate,
                dto.EndDate,
                dto.Priority,
                UserId = userId,
                IpAddress = clientIp
            }, transaction);

            if (rows == 0)
            {
                transaction.Rollback();
                return Result<int>.Failure("Notice not found for update.");
            }

            if (file is not null)
            {
                var savedPath = await SaveFileToDiskAsync(file, dto.NoticeId!);

                const string upsertFileSql = @"
                INSERT INTO noticeboard_file_path (notice_id, path, file_name, created_by, ip_address, flag)
                VALUES (@NoticeId, @Path, @FileName, @UserId, @IpAddress, 1)
                ON DUPLICATE KEY UPDATE
                    path = VALUES(path),
                    file_name = VALUES(file_name),
                    flag = 1,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress;";

                await connection.ExecuteAsync(upsertFileSql, new
                {
                    NoticeId = dto.NoticeId!,
                    Path = savedPath.Path,
                    FileName = savedPath.FileName,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);
            }

            transaction.Commit();
            return Result<int>.Success(dto.NoticeId!);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Result<int>.Failure($"Unable to update notice: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateActiveFlagAsync(int noticeId, bool status, string userId, string clientIp)
    {
        using var connection = connectionFactory.CreateConnection(HortiDb.Bts);

        const string sql = @"
            UPDATE noticeboard
            SET Status = @Flag, updated_by = @UserId
            WHERE notice_id = @NoticeId;";

        try
        {
            var rows = await connection.ExecuteAsync(sql, new
            {
                NoticeId = noticeId,
                Flag = status,
                UserId = userId,
                IpAddress = clientIp
            });

            return rows > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Notice not found.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Unable to update status: {ex.Message}");
        }
    }

    private const string TargetFolderName = "notifications_files";
    private async Task<(string FileName, string Path)> SaveFileToDiskAsync(IFormFile file, int noticeId)
    {
        var uploadDir = Path.Combine(env.WebRootPath, "docs", TargetFolderName);

        Directory.CreateDirectory(uploadDir);

        var sanitizedName = SafeFileName(file.FileName);

        var savedFileName = $"{noticeId}_{sanitizedName}";

        var fullPath = Path.Combine(uploadDir, savedFileName);

        await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);

        await file.CopyToAsync(stream);

        var dbPath = Path.Combine(TargetFolderName, savedFileName);

        return (sanitizedName, dbPath);
    }

    private static string SafeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();

        var cleaned = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());

        return string.IsNullOrWhiteSpace(cleaned) ? "file.pdf" : cleaned;
    }

}