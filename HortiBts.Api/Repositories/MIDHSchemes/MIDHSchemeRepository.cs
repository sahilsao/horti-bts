using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Schemes;
using HortiBts.Shared.Dtos.MIDHSchemes;
using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Repositories.MIDHSchemes
{
    public interface IMIDHSchemeRepository
    {
        Task<Result<List<SchemeTypeDto>>> GetSchemesTypesAsync();
        Task<IEnumerable<MIDHSchemeDto>> GetMIDHSchemesByTypeAsync(int stId);
        Task<Result<List<MIDHSchemeDto>>> GetMIDHSchemesListAsync();
        Task<Result<List<MIDHSchemeDto>>> GetMIDHSchemesBySchemeIDAsync(int schemeId);
        Task<Result<int>> SaveMIDHSchemeAsync(AddMIDHSchemeDto dto, IFormFile? file, string userId, string clientIp);
        Task<Result<int>> UpdateMIDHSchemeAsync(AddMIDHSchemeDto dto, IFormFile? file, string userId, string clientIp);
        Task<Result<bool>> UpdateMIDHActiveFlagAsync(int schemeId, bool flag, string userId);
    }

    public class MIDHSchemeRepository(IDbConnectionFactory dbFactory, IWebHostEnvironment env) : IMIDHSchemeRepository
    {
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

        public async Task<IEnumerable<MIDHSchemeDto>> GetMIDHSchemesByTypeAsync(int stId)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            const string Sql = @"
            SELECT 
                s.scheme_id AS SchemeId,
                s.midh_scheme_id AS MidhSchemeId,
                s.scheme_type_id AS SchemeTypeId,
                st.scheme_type_name_hi AS MidhSchemeTypeNameHi,
                st.scheme_type_name_en AS MidhSchemeTypeNameEn,
                s.scheme_name_en AS MidhSchemeNameEn,
                s.scheme_name_hi AS MidhSchemeNameHi,
                s.description_en AS MidhSchemeDescriptionEn,
                s.description_hi AS MidhSchemeDescriptionHi
            FROM mas_scheme_new s
            INNER
            JOIN mas_scheme_type st ON st.scheme_type_id = s.scheme_type_id
            WHERE s.flag = 1
            AND s.scheme_type_id = @StId";

            return await connection.QueryAsync<MIDHSchemeDto>(Sql, new { StId = stId });
        }

        public async Task<Result<List<MIDHSchemeDto>>> GetMIDHSchemesListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    s.scheme_id AS SchemeId,
                    s.midh_scheme_id AS MidhSchemeId,
                    s.scheme_type_id AS SchemeTypeId,
                    st.scheme_type_name_hi AS MidhSchemeTypeNameHi,
                    st.scheme_type_name_en AS MidhSchemeTypeNameEn,
                    s.scheme_name_en AS MidhSchemeNameEn,
                    s.scheme_name_hi AS MidhSchemeNameHi,
                    s.description_en AS MidhSchemeDescriptionEn,
                    s.description_hi AS MidhSchemeDescriptionHi
                FROM mas_scheme_new s
                INNER
                JOIN mas_scheme_type st ON st.scheme_type_id = s.scheme_type_id
                WHERE s.flag = 1
                AND s.scheme_type_id > 0 
                """;
                var result = await connection.QueryAsync<MIDHSchemeDto>(sql);
                return Result<List<MIDHSchemeDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHSchemeDto>>.Failure($"Failed to fetch MIDH schemes: {ex.Message}");
            }
        }

        public async Task<Result<List<MIDHSchemeDto>>> GetMIDHSchemesBySchemeIDAsync(int schemeId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    s.scheme_id AS SchemeId,
                    s.midh_scheme_id AS MidhSchemeId,
                    s.scheme_type_id AS SchemeTypeId,
                    st.scheme_type_name_hi AS MidhSchemeTypeNameHi,
                    st.scheme_type_name_en AS MidhSchemeTypeNameEn,
                    s.scheme_name_en AS MidhSchemeNameEn,
                    s.scheme_name_hi AS MidhSchemeNameHi,
                    s.description_en AS MidhSchemeDescriptionEn,
                    s.description_hi AS MidhSchemeDescriptionHi
                FROM mas_scheme_new s
                INNER
                JOIN mas_scheme_type st ON st.scheme_type_id = s.scheme_type_id
                WHERE s.flag = 1
                AND s.midh_scheme_id = @SchemeId
                """;
                var result = await connection.QueryAsync<MIDHSchemeDto>(sql, new { SchemeId = schemeId });
                return Result<List<MIDHSchemeDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHSchemeDto>>.Failure($"Failed to fetch MIDH schemes: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHSchemeAsync(AddMIDHSchemeDto dto, IFormFile? file, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string insertSql = @"
                INSERT INTO mas_scheme_new
                    (midh_scheme_id, scheme_type_id, code, scheme_name_hi, scheme_name_en, description_hi, description_en,
                    flag, created_by, ip_address)
                VALUES
                    (@MidhSchemeId, @SchemeTypeId, @MIDHSchemeCode, @MIDHSchemeNameHi, @MIDHSchemeNameEn, @MIDHSchemeDescriptionHi, @MIDHSchemeDescriptionEn,
                     'N', 'Y', @UserId, @IpAddress);
                SELECT LAST_INSERT_ID();";

                var insertedSchemeId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    dto.MIDHSchemeId,
                    dto.SchemeTypeId,
                    dto.MIDHSchemeCode,
                    dto.MIDHSchemeNameHi,
                    dto.MIDHSchemeNameEn,
                    dto.MIDHSchemeDescriptionHi,
                    dto.MIDHSchemeDescriptionEn,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                if (file is not null)
                {
                    var savedPath = await SaveFileToDiskAsync(file, insertedSchemeId);

                    const string filePathSql = @"
                    INSERT INTO mas_scheme_file_path_new (scheme_id, path, file_name, created_by, ip_address)
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

        public async Task<Result<int>> UpdateMIDHSchemeAsync(AddMIDHSchemeDto dto, IFormFile? file, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string updateSql = @"
                UPDATE mas_scheme_new
                SET midh_scheme_id = @MIDHSchemeId,
                    scheme_type_id = @SchemeTypeId,
                    code = @MIDHSchemeCode,
                    scheme_name = @MIDHSchemeNameHi,
                    scheme_name_en = @MIDHSchemeNameEn,
                    description_hi = @MIDHSchemeDescriptionHi,
                    description_en = @MIDHSchemeDescriptionEn,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE scheme_id = @SchemeId;";

                var rows = await connection.ExecuteAsync(updateSql, new
                {                    
                    dto.SchemeId,
                    dto.MIDHSchemeId,
                    dto.SchemeTypeId,
                    dto.MIDHSchemeNameHi,
                    dto.MIDHSchemeNameEn,
                    dto.MIDHSchemeDescriptionHi,
                    dto.MIDHSchemeDescriptionEn,
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
                    UPDATE mas_scheme_file_path_new
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
                    INSERT INTO mas_scheme_file_path_new (scheme_id, path, file_name, created_by, ip_address, flag)
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

        public async Task<Result<bool>> UpdateMIDHActiveFlagAsync(int schemeId, bool flag, string userId)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = @"
            UPDATE mas_scheme_new
            SET flag = @Flag, updated_by = @UserId
            WHERE scheme_id = @SchemeId;";

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

        private const string TargetFolderName = "scheme_files_new";
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
