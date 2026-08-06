using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Districts;

namespace HortiBts.Api.Repositories.Districts
{
    public interface IDistrictsRepository
    {
        /// <summary>Returns all districts</summary>
        Task<Result<DistrictsDto>>> GetDistrictsAsync();
    }

    public class DistrictsRepository(IDbConnectionFactory dbFactory) : IDistrictsRepository
    {
        public async Task<Result<DistrictsDto>>> GetDistrictsAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                select district_id      as DistrictId,
                       DistrictCensus,
                       DistrictName,
                       DistrictNameHindi,
                       div_id           as DivId,
                       c_bank_code      as CBankCode
                from rev_district
                order by DistrictName
                """;
                var result = await connection.QueryAsync<DistrictsDto>(sql);
                return Result<DistrictsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<DistrictsDto>>.Failure($"Failed to fetch districts: {ex.Message}");
            }
        }
    }
}
