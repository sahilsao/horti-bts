using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Villages;

namespace HortiBts.Api.Repositories.Villages
{
    public interface IVillagesRepository
    {
        /// <summary>Returns all villages from the specified sub-district</summary>
        Task<Result<List<VillagesDto>>> GetVillagesAsync(string SubDistrictCode);
        Task<Result<List<VillagesForRHEODto>>> GetVillagesForRHEOAsync(int DepartmentCode, string SubDistrictCode);
    }

    public class VillagesRepository(IDbConnectionFactory dbFactory) : IVillagesRepository
    {
        public async Task<Result<List<VillagesDto>>> GetVillagesAsync(string SubDistrictCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    rv.village_code AS VillageCode,
                    CONCAT(rv.village_name,'(',rv.Halka,')') AS VillageName,
                    CONCAT(rv.village_name_hi,'(',rv.Halka,')') AS VillageNameHi,
                    rv.TehsilCensus AS TehsilCensus,
                    rv.SubDistrictCodeCensus AS SubDistrictCode
                FROM rev_villages rv
                WHERE rv.SubDistrictCodeCensus=@SubDistrictCode
                ORDER BY rv.village_name
                """;
                var result = await connection.QueryAsync<VillagesDto>(sql, new { SubDistrictCode = SubDistrictCode });
                return Result<List<VillagesDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<VillagesDto>>.Failure($"Failed to fetch villages: {ex.Message}");
            }
        }

        public async Task<Result<List<VillagesForRHEODto>>> GetVillagesForRHEOAsync(int DepartmentCode, string SubDistrictCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    v.village_code AS VillageCode,
                    v.village_lgd_code AS VillageLgdCode,
                    v.village_name AS VillageName,
                    v.subdistrict_code AS SubdistrictCode,
                    rb.BlockNameEng AS subdistrict_name
                FROM view_all_villages v
                LEFT
                JOIN rev_block rb ON rb.subdistrict_code = v.subdistrict_code
                WHERE v.village_code NOT IN (
                SELECT 
                    a.village_code
                FROM officer_village_details a
                WHERE a.department_code = @DepartmentCode)
                AND v.subdistrict_code = @SubDistrictCode
                ORDER BY v.village_name
                """;
                var result = await connection.QueryAsync<VillagesForRHEODto>(sql, new { DepartmentCode = DepartmentCode, SubDistrictCode = SubDistrictCode });
                return Result<List<VillagesForRHEODto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<VillagesForRHEODto>>.Failure($"Failed to fetch villages for RHEO: {ex.Message}");
            }
        }
    }
}
