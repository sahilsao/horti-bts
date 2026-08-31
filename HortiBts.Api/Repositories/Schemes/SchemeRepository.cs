using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Schemes;
using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Repositories.Schemes
{
    public interface ISchemeRepository
    {
        Task<IEnumerable<SchemeDto>> GetByTypeAsync(int stId);
        Task<Result<List<SchemeTypeDto>>> GetSchemesTypesAsync();
        Task<Result<List<SchemeDto>>> GetSchemesListAsync();
        Task<Result<int>> SaveSchemeAsync(AddSchemeDto dto, IFormFile? file, string userId, string clientIp);
        Task<Result<int>> UpdateSchemeAsync(AddSchemeDto dto, IFormFile? file, string userId, string clientIp);
        Task<Result<bool>> UpdateBeneficiaryFlagAsync(int schemeId, bool flag, string userId);
        Task<Result<bool>> UpdateActiveFlagAsync(int schemeId, bool flag, string userId);

    }

    public class SchemeRepository(IDbConnectionFactory dbFactory, IWebHostEnvironment env) : ISchemeRepository
    {
        public async Task<IEnumerable<SchemeDto>> GetByTypeAsync(int stId)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            const string Sql = @"
            SELECT
            ms.s_id            AS SchemeId,
            ms.scheme_name     AS SchemeName,
            ms.scheme_name_en  AS SchemeNameEn,
            ms.scheme_code     AS SchemeCode,
            st.st_id           AS SchemeTypeId,
            st.scheme_name     AS SchemeTypeHi,
            st.scheme_name_en  AS SchemeTypeEn,
            ms.description_hi  AS SchemeDescriptionHi,
            ms.description_en  AS SchemeDescriptionEn,
            ms.isbeneficiary   AS IsBeneficiary,
            ms.flag            AS Flag
            FROM mas_scheme_horti ms
            INNER JOIN mas_scheme_horti st ON st.st_id = ms.st_id
            WHERE ms.flag = 'Y'
          AND ms.isbeneficiary = 'Y'
          AND ms.st_id = @StId";

            return await connection.QueryAsync<SchemeDto>(Sql, new { StId = stId });
        }

        public async Task<Result<List<SchemeTypeDto>>> GetSchemesTypesAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    ms.s_id AS SchemeId,
                    ms.scheme_name AS SchemeName,
                    ms.scheme_name_en AS SchemeNameEn,
                    ms.scheme_code AS SchemeCode,
                    ms.st_id AS SchemeTypeId,
                    ms.scheme_name AS SchemeTypeName,
                    ms.scheme_name_en AS SchemeTypeNameEn
                FROM mas_scheme_horti ms
                WHERE ms.flag='Y'
                AND ms.st_id=0
                """;
                var result = await connection.QueryAsync<SchemeTypeDto>(sql);
                return Result<List<SchemeTypeDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<SchemeTypeDto>>.Failure($"Failed to fetch scheme types: {ex.Message}");
            }
        }

        public async Task<Result<List<SchemeDto>>> GetSchemesListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    Row_number()
                         OVER(
                           ORDER BY ms.s_id DESC) AS Srno,
                    ms.s_id  AS SchemeId,
                    ms.st_id AS SchemeTypeId,
                    st.scheme_name  AS SchemeTypeHi,
                    st.scheme_name_en  AS SchemeTypeEn,
                    ms.scheme_name AS SchemeName,
                    ms.scheme_name_en AS SchemeNameEn,
                    ms.description_hi AS SchemeDescriptionHi,
                    ms.description_en AS SchemeDescriptionEn,
                    ms.isbeneficiary AS Isbeneficiary,
                    ms.flag AS Flag
                FROM mas_scheme_horti ms
                INNER
                JOIN mas_scheme_horti st ON st.s_id=ms.st_id
                WHERE ms.flag='Y'
                AND ms.st_id<>0 


                """;
                var result = await connection.QueryAsync<SchemeDto>(sql);
                return Result<List<SchemeDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<SchemeDto>>.Failure($"Failed to fetch schemes: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveSchemeAsync(AddSchemeDto dto, IFormFile? file, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string insertSql = @"
                INSERT INTO mas_scheme_horti
                    (st_id, scheme_name, scheme_name_en, description_hi, description_en,
                     isbeneficiary, flag, created_by, ip_address)
                VALUES
                    (@SchemeTypeId, @SchemeName, @SchemeNameEn, @SchemeDescriptionHi, @SchemeDescriptionEn,
                     'N', 'Y', @UserId, @IpAddress);
                SELECT LAST_INSERT_ID();";

                var insertedSchemeId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    dto.SchemeTypeId,
                    dto.SchemeName,
                    dto.SchemeNameEn,
                    dto.SchemeDescriptionHi,
                    dto.SchemeDescriptionEn,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                if (file is not null)
                {
                    var savedPath = await SaveFileToDiskAsync(file, insertedSchemeId);

                    const string filePathSql = @"
                    INSERT INTO mas_scheme_file_path (scheme_id, path, file_name, created_by, ip_address)
                    VALUES (@SchemeId, @Path, @FileName, @UserId, @IpAddress);";

                    await connection.ExecuteAsync(filePathSql, new
                    {
                        SchemeId = insertedSchemeId,
                        Path = savedPath.Path,
                        FileName = savedPath.FileName,
                        UserId = userId,
                        IpAddress = clientIp
                    }, transaction);
                }

                transaction.Commit();
                return Result<int>.Success(insertedSchemeId);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();
                return Result<int>.Failure("A scheme with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<int>.Failure($"Unable to save scheme: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateSchemeAsync(AddSchemeDto dto, IFormFile? file, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string updateSql = @"
                UPDATE mas_scheme_horti
                SET st_id = @SchemeTypeId,
                    scheme_name = @SchemeName,
                    scheme_name_en = @SchemeNameEn,
                    description_hi = @SchemeDescriptionHi,
                    description_en = @SchemeDescriptionEn,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE s_id = @SchemeId;";

                var rows = await connection.ExecuteAsync(updateSql, new
                {
                    dto.SchemeId,
                    dto.SchemeTypeId,
                    dto.SchemeName,
                    dto.SchemeNameEn,
                    dto.SchemeDescriptionHi,
                    dto.SchemeDescriptionEn,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                if (rows == 0)
                {
                    transaction.Rollback();
                    return Result<int>.Failure("Scheme not found for update.");
                }

                if (file is not null)
                {
                    var savedPath = await SaveFileToDiskAsync(file, dto.SchemeId!.Value);

                    // deactivate any existing active file record(s) for this scheme
                    const string deactivateSql = @"
                    UPDATE mas_scheme_file_path
                    SET flag = 0,
                        updated_by = @UserId,
                        updated_ip_address = @IpAddress
                    WHERE scheme_id = @SchemeId
                      AND flag = 1;";

                    await connection.ExecuteAsync(deactivateSql, new
                    {
                        SchemeId = dto.SchemeId!.Value,
                        UserId = userId,
                        IpAddress = clientIp
                    }, transaction);

                    // insert the new file as the active record
                    const string insertFileSql = @"
                    INSERT INTO mas_scheme_file_path (scheme_id, path, file_name, created_by, ip_address, flag)
                    VALUES (@SchemeId, @Path, @FileName, @UserId, @IpAddress, 1);";

                    await connection.ExecuteAsync(insertFileSql, new
                    {
                        SchemeId = dto.SchemeId!.Value,
                        Path = savedPath.Path,
                        FileName = savedPath.FileName,
                        UserId = userId,
                        IpAddress = clientIp
                    }, transaction);
                }

                transaction.Commit();
                return Result<int>.Success(dto.SchemeId!.Value);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<int>.Failure($"Unable to update scheme: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateBeneficiaryFlagAsync(int schemeId, bool flag, string userId)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = @"
            UPDATE mas_scheme_horti
            SET isbeneficiary = @Flag, updated_by = @UserId
            WHERE s_id = @SchemeId;";

            try
            {
                var rows = await connection.ExecuteAsync(sql, new
                {
                    SchemeId = schemeId,
                    Flag = flag ? "Y" : "N",
                    UserId = userId
                });

                return rows > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Scheme not found.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Unable to update flag: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateActiveFlagAsync(int schemeId, bool flag, string userId)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = @"
            UPDATE mas_scheme_horti
            SET flag = @Flag, updated_by = @UserId
            WHERE s_id = @SchemeId;";

            try
            {
                var rows = await connection.ExecuteAsync(sql, new
                {
                    SchemeId = schemeId,
                    Flag = flag ? "Y" : "N",
                    UserId = userId
                });

                return rows > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Scheme not found.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Unable to update flag: {ex.Message}");
            }
        }

        private const string TargetFolderName = "scheme_files";
        private async Task<(string FileName, string Path)> SaveFileToDiskAsync(IFormFile file, int schemeId)
        {
            var uploadDir = Path.Combine(env.WebRootPath, "docs", TargetFolderName);

            Directory.CreateDirectory(uploadDir);

            var sanitizedName = SafeFileName(file.FileName);

            var savedFileName = $"{schemeId}_{sanitizedName}";

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

}
