using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.MIDHComponents;
using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Repositories.MIDHComponents
{
    public interface IMIDHComponentRepository
    {
        Task<Result<List<MIDHComponentDto>>> GetMIDHComponentListAsync();
        Task<Result<List<MIDHComponentDto>>> GetMIDHComponentListByComponentIdAsync(int componentId);
        Task<Result<List<MIDHComponentDto>>> GetMIDHComponentListByComponentTypeIdAsync(int componentTypeId);
        Task<Result<int>> SaveMIDHComponentAsync(AddMIDHComponentDto dto, string userId, string clientIp);
        Task<Result<int>> UpdateMIDHComponentAsync(AddMIDHComponentDto dto, string userId, string clientIp);
        Task<Result<bool>> UpdateMIDHComponentActiveFlagAsync(int componentId, bool flag, string userId, string clientIp);

    }
    public class MIDHComponentRepository(IDbConnectionFactory dbFactory) : IMIDHComponentRepository
    {
        public async Task<Result<List<MIDHComponentDto>>> GetMIDHComponentListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    c.component_id AS ComponentId,
                    c.component_type_id AS MidhComponentTypeId,
                    c.midh_component_id AS MidhComponentId,
                    c.component_name_en AS MidhComponentNameEn,
                    c.component_name_hi AS MidhComponentNameHi,
                    c.description_en AS MidhComponentDescriptionEn,
                    c.description_hi AS MidhComponentDescriptionHi,
                    ct.component_type_name_en AS MidhComponentTypeNameEn,
                    ct.component_type_name_hi AS MidhComponentTypeNameHi,
                    ct.scheme_id AS MidhSchemeId,
                    s.scheme_type_id AS MidhSchemeTypeId,
                    s.scheme_name_en AS MidhSchemeNameEn,
                    s.scheme_name_hi AS MidhSchemeNameHi,
                    c.flag AS Flag
                FROM mas_component_new c
                INNER
                JOIN mas_component_type_new ct ON ct.component_type_id = c.component_type_id
                INNER
                JOIN mas_scheme_new s ON s.scheme_id = ct.scheme_id
                """;
                var result = await connection.QueryAsync<MIDHComponentDto>(sql);
                return Result<List<MIDHComponentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentDto>>.Failure($"Failed to fetch MIDH Component : {ex.Message}");
            }
        }

        public async Task<Result<List<MIDHComponentDto>>> GetMIDHComponentListByComponentIdAsync(int componentId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    c.component_id As ComponentId,
                    c.component_type_id As MidhComponentTypeId,
                    c.midh_component_id As MidhComponentId,
                    c.component_name_en As MidhComponentNameEn,
                    c.component_name_hi As MidhComponentNameHi,
                    c.description_en As MidhComponentDescriptionEn,
                    c.description_hi As MidhComponentDescriptionHi,
                    ct.component_type_name_en As MidhComponentTypeNameEn,
                    ct.component_type_name_hi As MidhComponentTypeNameHi,
                    ct.scheme_id As MidhSchemeId,
                    s.scheme_type_id As MidhSchemeTypeId,
                    s.scheme_name_en As MidhSchemeNameEn,
                    s.scheme_name_hi As MidhSchemeNameHi,
                    c.flag As Flag
                FROM mas_component_new c
                INNER JOIN mas_component_type_new ct ON ct.component_type_id = c.component_type_id
                INNER JOIN mas_scheme_new s ON s.scheme_id = ct.scheme_id
                WHERE c.flag = 1 AND c.component_id =@ComponentId
                """;
                var result = await connection.QueryAsync<MIDHComponentDto>(sql, new { ComponentId = componentId });
                return Result<List<MIDHComponentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentDto>>.Failure($"Failed to fetch MIDH Component : {ex.Message}");
            }
        }

        public async Task<Result<List<MIDHComponentDto>>> GetMIDHComponentListByComponentTypeIdAsync(int componentTypeId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    c.component_id As ComponentId,
                    c.component_type_id As MidhComponentTypeId,
                    c.midh_component_id As MidhComponentId,
                    c.component_name_en As MidhComponentNameEn,
                    c.component_name_hi As MidhComponentNameHi,
                    c.description_en As MidhComponentDescriptionEn,
                    c.description_hi As MidhComponentDescriptionHi,
                    ct.component_type_name_en As MidhComponentTypeNameEn,
                    ct.component_type_name_hi As MidhComponentTypeNameHi,
                    ct.scheme_id As MidhSchemeId,
                    s.scheme_type_id As MidhSchemeTypeId,
                    s.scheme_name_en As MidhSchemeNameEn,
                    s.scheme_name_hi As MidhSchemeNameHi,
                    c.flag As Flag
                FROM mas_component_new c
                INNER JOIN mas_component_type_new ct ON ct.component_type_id = c.component_type_id
                INNER JOIN mas_scheme_new s ON s.scheme_id = ct.scheme_id
                WHERE c.flag = 1 AND c.component_type_id =@ComponentTypeId
                """;
                var result = await connection.QueryAsync<MIDHComponentDto>(sql, new { ComponentTypeId = componentTypeId });
                return Result<List<MIDHComponentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentDto>>.Failure($"Failed to fetch MIDH Component : {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHComponentAsync(AddMIDHComponentDto dto, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string insertSql = @"
                INSERT INTO mas_component_new 
                (component_type_id, 
                midh_component_id, 
                component_name_en,
                component_name_hi, 
                description_en, 
                description_hi, 
                flag, 
                created_by, 
                ip_address )
                    VALUES
                    (@MidhComponentTypeId,
                    @MidhComponentId,
                    @MidhComponentNameEn, 
                    @MidhComponentNameHi,
                    @MidhComponentDescriptionEn, 
                    @MidhComponentDescriptionHi, 
                    '1', 
                    @UserId,
                    @IpAddress);
                SELECT LAST_INSERT_ID();";

                var insertedSchemeId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    dto.MidhComponentTypeId,
                    dto.MidhComponentId,
                    dto.MidhComponentNameEn,
                    dto.MidhComponentNameHi,
                    dto.MidhComponentDescriptionEn,
                    dto.MidhComponentDescriptionHi,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                transaction.Commit();
                return Result<int>.Success(insertedSchemeId);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();
                return Result<int>.Failure("A MIDH Component  with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<int>.Failure($"Unable to save MIDH Component : {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHComponentAsync(AddMIDHComponentDto dto, string userId, string clientIp)
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
                    FROM mas_component_new
                    WHERE component_id = @ComponentId;";

                var exists = await connection.ExecuteScalarAsync<int>(
                    existsSql,
                    new
                    {
                        dto.ComponentId
                    },
                    transaction);

                if (exists == 0)
                {
                    transaction.Rollback();

                    return Result<int>.Failure("MIDH Component not found for update.");
                }

                const string updateSql = @"
                UPDATE mas_component_new
                SET
                    component_type_id = @MidhComponentTypeId,
                    midh_component_id = @MidhComponentId,
                    component_name_en = @MidhComponentNameEn,
                    component_name_hi = @MidhComponentNameHi,
                    description_en = @MidhComponentDescriptionEn,
                    description_hi = @MidhComponentDescriptionHi,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE component_id = @ComponentId;";

                await connection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        dto.MidhComponentTypeId,
                        dto.MidhComponentId,
                        dto.MidhComponentNameEn,
                        dto.MidhComponentNameHi,
                        dto.MidhComponentDescriptionEn,
                        dto.MidhComponentDescriptionHi,
                        UserId = userId,
                        IpAddress = clientIp,
                        dto.ComponentId
                    },
                    transaction);

                transaction.Commit();

                return Result<int>.Success(dto.ComponentId!.Value);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();

                return Result<int>.Failure("A MIDH Component  with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<int>.Failure(
                    $"Unable to update MIDH Component : {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMIDHComponentActiveFlagAsync(int componentId, bool flag, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string UpdateFlagSql = @"
                UPDATE mas_component_new
                SET
                    flag =  @Flag,
                    updated_by = @UserId,
                    ip_address = @IpAddress
                WHERE component_id = @ComponentId";

                var rows = await connection.ExecuteAsync(
                    UpdateFlagSql,
                    new
                    {
                        ComponentId = componentId,
                        Flag = flag ? 1 : 0,
                        UserId = userId,
                        IpAddress = clientIp
                    },
                    transaction);

                if (rows == 0)
                {
                    transaction.Rollback();

                    return Result<bool>.Failure("MIDH Component  not found or already deactivated.");
                }

                transaction.Commit();

                return Result<bool>.Success(flag);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<bool>.Failure($"Unable to deactivate MIDH Component : {ex.Message}");
            }
        }
    }
}
