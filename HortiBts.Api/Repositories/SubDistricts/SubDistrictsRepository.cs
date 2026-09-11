using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.SubDistricts;

namespace HortiBts.Api.Repositories.SubDistricts
{
    public interface ISubDistrictsRepository
    {
        /// <summary>Returns all sub-districts for the specified district</summary>
        Task<Result<List<SubDistrictsDto>>> GetSubDistrictsAsync(string DistrictCode);
    }

    public class SubDistrictsRepository(IDbConnectionFactory dbFactory) : ISubDistrictsRepository
    {
        public async Task<Result<List<SubDistrictsDto>>> GetSubDistrictsAsync(string DistrictCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    rb.subdistrict_code AS SubDistrictCode,
                    rb.BlockNameEng AS SubDistrictName,
                    rb.BlockNameHin AS SubDistrictNameHi
                FROM rev_block rb
                WHERE rb.DistCodeCensus=@DistrictCode
                ORDER BY rb.BlockNameEng
                """;
                var result = await connection.QueryAsync<SubDistrictsDto>(sql, new { DistrictCode = DistrictCode });
                return Result<List<SubDistrictsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<SubDistrictsDto>>.Failure($"Failed to fetch sub-districts: {ex.Message}");
            }
        }
    }
}
