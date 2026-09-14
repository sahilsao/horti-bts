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

        Task<Result<List<RheoOfficerMappedVillagesDto>>> GetRheoOfficerMappedVillagesListAsync(int departmentCode, int officerCode);
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

        public async Task<Result<List<RheoOfficerMappedVillagesDto>>> GetRheoOfficerMappedVillagesListAsync(int departmentCode, int officerCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    SELECT
                    b.village_code AS VillageCode,
                    (
                        CASE WHEN b.Halka IS NULL THEN b.village_name ELSE CONCAT(
                            b.village_name, ' (', b.Halka, ')'
                        ) END
                    ) AS VillageName ,
                    b.Halka,
                    b.subdistrict_code AS SubDistrictCode,
                    rb.BlockNameEng AS SubDistrictName,
                    b.DistCodeCensus AS DistrictCode,
                    b.district_id AS DistrictId,
                    b.DistrictName AS DistrictName
                FROM
                    officer_village_details a
                    INNER JOIN view_all_villages b ON a.village_code = b.village_code
                    INNER JOIN rev_block rb ON rb.subdistrict_code = b.subdistrict_code
                WHERE
                    a.officer_code = @OfficerCode
                    AND a.department_code = @DepartmentCode
                ORDER BY
                    b.village_name
                """;
                var result = await connection.QueryAsync<RheoOfficerMappedVillagesDto>(sql, new { DepartmentCode = departmentCode, OfficerCode = officerCode });
                return Result<List<RheoOfficerMappedVillagesDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficerMappedVillagesDto>>.Failure($"Failed to fetch rheo officer's mapped villages: {ex.Message}");
            }
        }
    }
}
