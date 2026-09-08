using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.MIDHComponents;
using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Repositories.MIDHComponents
{
    public interface IMIDHComponentTypeRepository
    {
        Task<Result<List<MIDHComponentTypeDto>>> GetMIDHComponentTypeListAsync();
        Task<Result<int>> SaveMIDHComponentTypeAsync(AddMIDHComponentTypeDto dto, string userId, string clientIp);
        Task<Result<int>> UpdateMIDHComponentTypeAsync(AddMIDHComponentTypeDto dto, string userId, string clientIp);
        Task<Result<int>> UpdateMIDHComponentTypeActiveFlagAsync(int componentId, bool flag, string userId, string clientIp);

    }
    public class MIDHComponentTypeRepository(IDbConnectionFactory dbFactory) : IMIDHComponentTypeRepository
    {
        public async Task<Result<List<MIDHComponentTypeDto>>> GetMIDHComponentTypeListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    ct.component_type_id AS ComponentTypeId,
                    ct.scheme_id AS MIDHSchemeId,
                    s.scheme_name_en AS MIDHSchemeNameEn,
                    s.scheme_name_hi AS MIDHSchemeNameHi,
                    ct.component_type_name_en AS MIDHComponentTypeNameEn,
                    ct.component_type_name_hi AS MIDHComponentTypeNameHi,
                    s.scheme_type_id AS SchemeTypeId,
                    ct.description_en AS MIDHComponentDescriptionEn,
                    ct.description_hi AS MIDHComponentDescriptionHi,
                    ct.midh_component_type_id AS MidhComponentTypeId
                FROM mas_component_type_new ct
                INNER
                JOIN mas_scheme_new s ON s.scheme_id=ct.scheme_id
                WHERE ct.flag=1
                """;
                var result = await connection.QueryAsync<MIDHComponentTypeDto>(sql);
                return Result<List<MIDHComponentTypeDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentTypeDto>>.Failure($"Failed to fetch MIDH Component Type: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHComponentTypeAsync(AddMIDHComponentTypeDto dto, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string insertSql = @"
                INSERT INTO mas_component_type_new 
                (scheme_id, 
                midh_component_type_id, 
                component_type_name_en,
                component_type_name_hi, 
                description_en, 
                description_hi, 
                flag, 
                created_by, 
                ip_address )
                    VALUES
                    (@MidhSchemeId,
                    @MidhComponentTypeId,
                    @MidhComponentTypeNameEn, 
                    @MidhComponentTypeNameHi,
                    @MidhComponentDescriptionEn, 
                    @MidhComponentDescriptionHi, 
                    '1', 
                    @UserId,
                    @IpAddress);
                SELECT LAST_INSERT_ID();";

                var insertedSchemeId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    dto.MidhSchemeId,
                    dto.MidhComponentTypeId,
                    dto.MidhComponentTypeNameEn,
                    dto.MidhComponentTypeNameHi,
                    dto.MidhComponentTypeDescriptionEn,
                    dto.MidhComponentTypeDescriptionHi,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                transaction.Commit();
                return Result<int>.Success(insertedSchemeId);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();
                return Result<int>.Failure("A MIDH Component Type with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<int>.Failure($"Unable to save MIDH Component Type: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHComponentTypeAsync(AddMIDHComponentTypeDto dto, string userId, string clientIp)
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
                    FROM mas_component_type_new
                    WHERE component_type_id = @ComponentTypeId;";

                var exists = await connection.ExecuteScalarAsync<int>(
                    existsSql,
                    new
                    {
                        dto.ComponentTypeId
                    },
                    transaction);

                if (exists == 0)
                {
                    transaction.Rollback();

                    return Result<int>.Failure("MIDH Component Type not found for update.");
                }

                const string updateSql = @"
                UPDATE mas_component_type_new
                SET
                    scheme_id = @SchemeId,
                    midh_component_type_id = @ComponentNameEn,
                    component_type_name_en = @ComponentNameHi,
                    component_type_name_hi = @ComponentDescriptionEn,
                    description_en = @ComponentDescriptionEn,
                    description_hi = @ComponentDescriptionHi,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE component_type_id = @ComponentTypeId;";

                await connection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        dto.MidhSchemeId,
                        dto.MidhComponentTypeId,
                        dto.MidhComponentTypeNameEn,
                        dto.MidhComponentTypeNameHi,
                        dto.MidhComponentTypeDescriptionEn,
                        dto.MidhComponentTypeDescriptionHi,
                        UserId = userId,
                        IpAddress = clientIp
                    },
                    transaction);

                transaction.Commit();

                return Result<int>.Success(dto.ComponentTypeId!.Value);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();

                return Result<int>.Failure("A MIDH Component Type with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<int>.Failure(
                    $"Unable to update MIDH Component Type: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHComponentTypeActiveFlagAsync(int componentTypeId, bool flag, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string UpdateFlagSql = @"
                UPDATE mas_component_type_new
                SET
                    flag =  @Flag,
                    updated_by = @UserId,
                    ip_address = @IpAddress
                WHERE component_type_id = @ComponentTypeId";

                var rows = await connection.ExecuteAsync(
                    UpdateFlagSql,
                    new
                    {
                        ComponentTypeId = componentTypeId,
                        Flag = flag ? 1 : 0,
                        UserId = userId,
                        IpAddress = clientIp
                    },
                    transaction);

                if (rows == 0)
                {
                    transaction.Rollback();

                    return Result<int>.Failure("MIDH Component Type not found or already deactivated.");
                }

                transaction.Commit();

                return Result<int>.Success(componentTypeId);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<int>.Failure($"Unable to deactivate MIDH Component Type: {ex.Message}");
            }
        }
    }
}
