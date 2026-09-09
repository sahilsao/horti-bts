using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using HortiBts.Api.Data;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Schemes;
using MySqlConnector;
using System.ComponentModel;
using System.Data;
using System.Net;

namespace HortiBts.Api.Repositories.Components
{
    public interface IComponentRepository
    {
        Task<Result<List<ComponentDto>>> GetComponentListAsync();
        Task<Result<int>> SaveComponentAsync(AddComponentDto dto, string userId, string clientIp);
        Task<Result<int>> UpdateComponentAsync(AddComponentDto dto, string userId, string clientIp);
        Task<Result<bool>> UpdateComponentActiveFlagAsync(int componentId, bool flag, string userId, string clientIp);
    }
    public class ComponentRepository(IDbConnectionFactory dbFactory) : IComponentRepository
    {
        public async Task<Result<List<ComponentDto>>> GetComponentListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    mc.c_id AS ComponentId,
                    mc.cname AS ComponentName,
                    mc.cname_hi  AS ComponentNameHi,
                    mc.description_en AS ComponentDescriptionEn,
                    mc.description_hi AS ComponentDescriptionHi,
                    mc.unit_id AS ComponentUnitId,
                    mc.s_id AS SchemeId,
                    ms.scheme_name AS SchemeName,
                    ms.scheme_name_en AS SchemeNameEn,
                    ms.st_id  AS SchemeTypeId,
                    CASE WHEN mc.flag = 'Y' THEN 1 ELSE 0 END AS ComponentFlag
                FROM mas_component_horti mc
                INNER JOIN mas_scheme_horti ms ON ms.s_id=mc.s_id
                WHERE mc.flag='Y'
                """;
                var result = await connection.QueryAsync<ComponentDto>(sql);
                return Result<List<ComponentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<ComponentDto>>.Failure($"Failed to fetch components: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveComponentAsync(AddComponentDto dto, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string insertSql = @"
                INSERT INTO mas_component_horti (s_id, cname, cname_hi, unit_id, description_en, description_hi, flag, created_by, ip_address )
                    VALUES
                    (@SchemeId, @ComponentNameEn, @ComponentNameHi, @ComponentUnitId, @ComponentDescriptionEn, @ComponentDescriptionHi, 'Y', @UserId, @IpAddress);
                SELECT LAST_INSERT_ID();";

                var insertedSchemeId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    dto.SchemeId,
                    dto.ComponentNameEn,
                    dto.ComponentNameHi,
                    dto.ComponentUnitId,
                    dto.ComponentDescriptionEn,
                    dto.ComponentDescriptionHi,
                    UserId = userId,
                    IpAddress = clientIp
                }, transaction);

                transaction.Commit();
                return Result<int>.Success(insertedSchemeId);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();
                return Result<int>.Failure("A component with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<int>.Failure($"Unable to save component: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateComponentAsync(AddComponentDto dto, string userId, string clientIp)
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
                    FROM mas_component_horti
                    WHERE c_id = @ComponentId;";

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

                    return Result<int>.Failure("Component not found for update.");
                }

                const string updateSql = @"
                UPDATE mas_component_horti
                SET
                    s_id = @SchemeId,
                    cname = @ComponentNameEn,
                    cname_hi = @ComponentNameHi,
                    unit_id = @ComponentUnitId,
                    description_en = @ComponentDescriptionEn,
                    description_hi = @ComponentDescriptionHi,
                    updated_by = @UserId,
                    updated_ip_address = @IpAddress
                WHERE c_id = @ComponentId;";

                await connection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        dto.SchemeId,
                        dto.ComponentId,
                        dto.ComponentNameEn,
                        dto.ComponentNameHi,
                        dto.ComponentUnitId,
                        dto.ComponentDescriptionEn,
                        dto.ComponentDescriptionHi,
                        UserId = userId,
                        IpAddress = clientIp
                    },
                    transaction);

                transaction.Commit();

                return Result<int>.Success(dto.ComponentId!.Value);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                transaction.Rollback();

                return Result<int>.Failure("A component with this name already exists.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<int>.Failure(
                    $"Unable to update component: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateComponentActiveFlagAsync(int componentId, bool flag, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string updateActiveFlagSql = @"               

                UPDATE mas_component_horti
                SET flag = @Flag, updated_by = @UserId,
                    ip_address = @IpAddress
                WHERE c_id = @ComponentId ";
                var rows = await connection.ExecuteAsync(
                    updateActiveFlagSql,
                    new
                    {
                        ComponentId = componentId,
                        Flag = flag ? "Y" : "N",
                        UserId = userId,
                        IpAddress = clientIp
                    },
                    transaction);

                if (rows == 0)
                {
                    transaction.Rollback();

                    return Result<bool>.Failure("Component not found or already deactivated.");
                }

                transaction.Commit();

                return Result<bool>.Success(flag);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Result<bool>.Failure($"Unable to deactivate component: {ex.Message}");
            }
        }
    }
}
