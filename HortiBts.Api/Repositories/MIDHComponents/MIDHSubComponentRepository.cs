using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.MIDHComponents;
using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Repositories.MIDHComponents
{
    public interface IMIDHSubComponentRepository
    {
        Task<Result<List<MIDHSubComponentDto>>> GetMIDHSubComponentListAsync();
        Task<Result<int>> SaveMIDHSubComponentAsync(AddMIDHSubComponentDto dto, string userId, string clientIp);
        Task<Result<int>> UpdateMIDHSubComponentAsync(AddMIDHSubComponentDto dto, string userId, string clientIp);
        Task<Result<bool>> UpdateMIDHSubComponentActiveFlagAsync(int subComponentId, bool flag, string userId, string clientIp);

    }
    public class MIDHSubComponentRepository(IDbConnectionFactory dbFactory) : IMIDHSubComponentRepository
    {
        public async Task<Result<List<MIDHSubComponentDto>>> GetMIDHSubComponentListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    c.sub_component_id AS SubComponentId,
                    c.component_id AS ComponentId,
                    c.sub_component_name_en AS MidhSubComponentNameEn,
                    c.sub_component_name_hi AS MidhSubComponentNameHi,
                    c.description_en AS MidhSubComponentDescriptionEn,
                    c.description_hi AS MidhSubComponentDescriptionHi,
                    c.midh_sub_component_id AS MidhSubComponentId,
                    cc.component_name_en AS MidhComponentNameEn,
                    cc.component_name_hi AS MidhComponentNameHi,
                    ct.component_type_id AS MidhComponentTypeId,
                    ct.component_type_name_en AS MidhComponentTypeNameEn,
                    ct.component_type_name_hi AS MidhComponentTypeNameHi,
                    s.scheme_type_id AS MidhSchemeTypeId,
                    s.scheme_id AS MidhSchemeId,
                    s.scheme_name_en AS MidhSchemeNameEn,
                    s.scheme_name_hi AS MidhSchemeNameHi,
                    c.flag AS Flag
                FROM mas_sub_component_new c
                INNER
                JOIN mas_component_new cc ON cc.component_id = c.component_id
                INNER
                JOIN mas_component_type_new ct ON ct.component_type_id = cc.component_type_id
                INNER
                JOIN mas_scheme_new s ON s.scheme_id = ct.scheme_id
                """;
                var result = await connection.QueryAsync<MIDHSubComponentDto>(sql);
                return Result<List<MIDHSubComponentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHSubComponentDto>>.Failure($"Failed to fetch MIDH Sub Component : {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHSubComponentAsync(AddMIDHSubComponentDto dto, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string insertSql = @"
                INSERT INTO mas_sub_component_new 
                (component_id,
                midh_sub_component_id,
                sub_component_name_en,
                sub_component_name_hi, 
                description_en, 
                description_hi, 
                flag, 
                created_by, 
                ip_address )
                    VALUES
                    (@MidhComponentId,
                    @MidhSubComponentId,
                    @MidhSubComponentNameEn, 
                    @MidhSubComponentNameHi,
                    @MidhSubComponentDescriptionEn, 
                    @MidhSubComponentDescriptionHi, 
                    '1', 
                    @UserId,
                    @IpAddress);
                SELECT LAST_INSERT_ID();";

                var insertedSchemeId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    dto.MidhComponentId,
                    dto.MidhSubComponentId,
                    dto.MidhSubComponentNameEn,
                    dto.MidhSubComponentNameHi,
                    dto.MidhSubComponentDescriptionEn,
                    dto.MidhSubComponentDescriptionHi,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                transaction.Commit();
                return Result<int>.Success(insertedSchemeId);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();
                return Result<int>.Failure("A MIDH Sub Component with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<int>.Failure($"Unable to save MIDH Sub Component : {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHSubComponentAsync(AddMIDHSubComponentDto dto, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // First check that the component exists
                const string existsSql = @"
                    SELECT COUNT(*)
                    FROM mas_sub_component_new
                    WHERE sub_component_id = @SubComponentId;";

                var exists = await connection.ExecuteScalarAsync<int>(
                    existsSql,
                    new
                    {
                        dto.SubComponentId
                    },
                    transaction);

                if (exists == 0)
                {
                    transaction.Rollback();

                    return Result<int>.Failure("MIDH Sub Component not found for update.");
                }

                const string updateSql = @"
                UPDATE mas_sub_component_new
                SET                    
                    midh_sub_component_id = @MidhSubComponentId,
                    component_id = @MidhComponentId,
                    sub_component_name_en = @MidhSubComponentNameEn,
                    sub_component_name_hi = @MidhSubComponentNameHi,
                    description_en = @MidhSubComponentDescriptionEn,
                    description_hi = @MidhSubComponentDescriptionHi,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE sub_component_id = @SubComponentId;";

                await connection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        dto.MidhSubComponentId,
                        dto.MidhComponentId,
                        dto.MidhSubComponentNameEn,
                        dto.MidhSubComponentNameHi,
                        dto.MidhSubComponentDescriptionEn,
                        dto.MidhSubComponentDescriptionHi,
                        UserId = userId,
                        IpAddress = clientIp,
                        dto.SubComponentId
                    },
                    transaction);

                transaction.Commit();

                return Result<int>.Success(dto.SubComponentId!.Value);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();

                return Result<int>.Failure("A MIDH Sub Component with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<int>.Failure(
                    $"Unable to update MIDH Sub Component : {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMIDHSubComponentActiveFlagAsync(int subComponentId, bool flag, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string UpdateFlagSql = @"
                UPDATE mas_sub_component_new
                SET
                    flag =  @Flag,
                    updated_by = @UserId,
                    ip_address = @IpAddress
                WHERE sub_component_id = @SubComponentId";

                var rows = await connection.ExecuteAsync(
                    UpdateFlagSql,
                    new
                    {
                        SubComponentId = subComponentId,
                        Flag = flag ? 1 : 0,
                        UserId = userId,
                        IpAddress = clientIp
                    },
                    transaction);

                if (rows == 0)
                {
                    transaction.Rollback();

                    return Result<bool>.Failure("MIDH Sub Component not found or already deactivated.");
                }

                transaction.Commit();

                return Result<bool>.Success(flag);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<bool>.Failure($"Unable to deactivate MIDH Sub Component : {ex.Message}");
            }
        }
    }
}
