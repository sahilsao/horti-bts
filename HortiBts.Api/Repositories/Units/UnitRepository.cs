using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Districts;
using HortiBts.Shared.Dtos.Units;

namespace HortiBts.Api.Repositories.Units
{
    public interface IUnitRepository
    {
        /// <summary>Returns all units</summary>
        Task<Result<List<UnitDto>>> GetUnitsAsync();
    }

    public class UnitRepository(IDbConnectionFactory dbFactory) : IUnitRepository
    {
        public async Task<Result<List<UnitDto>>> GetUnitsAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    unit_id AS UnitId,
                    unit_name AS UnitName,
                    benefit_type_id AS BenefitTypeId,
                    upload_date AS UploadDate,
                    flag AS Flag
                FROM mas_units WHERE flag = 'Y' ORDER BY unit_id
                """;
                var result = await connection.QueryAsync<UnitDto>(sql);
                return Result<List<UnitDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<UnitDto>>.Failure($"Failed to fetch units: {ex.Message}");
            }
        }
    }
}