using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Officers;
using HortiBts.Shared.Dtos.SubDistricts;

namespace HortiBts.Api.Repositories.Officers
{
    public interface IOfficersRepository
    {
        /// <summary>Returns all rheo officers for the specified sub-district</summary>
        Task<Result<List<RheoOfficersDto>>> GetOfficersListBySubDistrictCodeAsync(int departmentCode, int subDistrictCode);
    }

    public class OfficersRepository(IDbConnectionFactory dbFactory) : IOfficersRepository
    {
        public async Task<Result<List<RheoOfficersDto>>> GetOfficersListBySubDistrictCodeAsync(int departmentCode, int subDistrictCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT
                    mr.officer_code AS OfficerCode,
                    mr.name AS OfficerName,
                    mr.mobile_no AS MobileNo,
                    COALESCE(mr.alternate_mobile_no, '-') AS AlternateMobileNo,
                    COALESCE(mr.email, '-') AS Email,
                    mr.charge_date AS ChargeTakenDate,
                    v.subdistrict_code AS SubDistrictCode,
                    v.subdistrict_name AS SubDistrictName,
                    v.DistCodeCensus AS DistrictCodeCensus,
                    v.DistrictName AS DistrictName
                    FROM
                    view_all_villages v
                    INNER JOIN officer_village_details ovd ON ovd.village_code = v.village_code
                    INNER JOIN mas_raeo mr ON mr.officer_code = ovd.officer_code
                    WHERE
                    v.subdistrict_code = @SubDistrictCode
                    AND mr.usertype = @DepartmentCode
                    GROUP BY
                    ovd.officer_code
                    ORDER BY
                    mr.name
                """;
                var result = await connection.QueryAsync<RheoOfficersDto>(sql, new { DepartmentCode = departmentCode, SubDistrictCode = subDistrictCode });
                return Result<List<RheoOfficersDto>>.Success(result.ToList());
            }       
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch officers: {ex.Message}");
            }
        }
    }
}
