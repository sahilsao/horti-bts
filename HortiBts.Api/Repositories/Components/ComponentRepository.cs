using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Schemes;
using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Repositories.Components
{
    public interface IComponentRepository
    {
        Task<Result<List<ComponentDto>>> GetComponentListAsync();
        Task<Result<int>> SaveComponentAsync(AddComponentDto dto, string userId, string clientIp);
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
                    Row_number()
                         OVER(
                           ORDER BY ms.s_id DESC) AS Srno,
                    mc.c_id AS ComponentId,
                    mc.cname AS ComponentName,
                    mc.cname_hi  AS ComponentNameHi,
                    mc.description_en AS ComponentDescriptionEn,
                    mc.description_hi AS ComponentDescriptionHi,
                    mc.unit_id AS ComponentUnitId,
                    mc.s_id AS SchemeId,
                    ms.scheme_name AS SchemeName,
                    ms.scheme_name_en AS SchemeNameEn,
                    ms.st_id  AS SchemeTypeId
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
                SELECT LAST_INSERT ID();";

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
    }
}
