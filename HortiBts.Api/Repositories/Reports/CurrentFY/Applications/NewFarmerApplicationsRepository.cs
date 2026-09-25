using System;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;

namespace HortiBts.Api.Repositories.Reports.CurrentFY.Applications;

public interface INewFarmerApplicationsRepository
{
    Task<Result<List<DistrictWiseFarmerApplicationsDto>>> GetRptOfDistrictWiseApplicationsAsync(int financialYear);
    Task<Result<List<BlockWiseFarmerApplicationsDto>>> GetRptOfBlockWiseApplicationsAsync(int districtCode, int financialYear);
    Task<Result<List<RHEOWiseFarmerApplicationsDto>>> GetRptOfRheoWiseApplicationsAsync(int departmentCode, int districtCode, int subDistrictCode, int financialYear);
    Task<Result<List<VillageWiseFarmerApplicationsDto>>> GetRptOfVillageWiseApplicationsAsync(int officerCode, int financialYear);
    Task<Result<List<FarmerWiseFarmerApplicationsDto>>> GetRptOfFarmerWiseApplicationsAsync(int districtCode, int subDistrictCode, int villageCode, int officerCode, int financialYear);
}
public class NewFarmerApplicationsRepository(IDbConnectionFactory connectionFactory) : INewFarmerApplicationsRepository
{
    public async Task<Result<List<DistrictWiseFarmerApplicationsDto>>> GetRptOfDistrictWiseApplicationsAsync(int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                IFNULL(r.target_count, 0) AS target,
                SUM(IFNULL(s.f_count, 0)) AS f_count,
                SUM(IFNULL(s.s_count, 0)) AS s_count,
                TRUNCATE(SUM(IFNULL(s.area, 0)), 3) AS area,
                SUM(ifnull(s.r_approved,0)) r_approved,
                SUM(ifnull(s.d_approved,0)) d_approved,
                va.DistCodeCensus AS district_code,
                rd.DistrictNameHindi AS district_name_hi,
                rd.DistrictName AS district_name_en
            FROM view_all_villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            LEFT
            JOIN (
            SELECT 
                COUNT(DISTINCT gs.application_id) AS f_count,
                COUNT(gs.sd_id) AS s_count,
                COUNT(if(fd.rheo_approval_status=1, fd.application_id, NULL)) r_approved,
                COUNT(if(fd.ddh_approval_status=1, fd.application_id, NULL)) d_approved,
                SUM(gs.area) AS area,
                gs.village_code
            FROM temp_scheme_details gs
            INNER
            JOIN temp_farmer_application fd ON fd.application_id = gs.application_id
            WHERE gs.financial_year = @FinancialYear
            GROUP BY gs.village_code ) s ON s.village_code = va.village_code
            LEFT
            JOIN reg_target r ON r.user_id = rd.DistrictCensus
            AND r.financial_year = @FinancialYear
            GROUP BY va.DistCodeCensus
            ORDER BY va.DistrictName";

        var result = await connection.QueryAsync<DistrictWiseFarmerApplicationsDto>(Sql, new { FinancialYear = financialYear });
        return Result<List<DistrictWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<BlockWiseFarmerApplicationsDto>>> GetRptOfBlockWiseApplicationsAsync(int districtCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT IFNULL(r.target_count, 0) AS target,
                SUM(IFNULL(s.f_count, 0)) AS f_count,
                SUM(IFNULL(s.s_count, 0)) AS s_count,
                TRUNCATE(SUM(IFNULL(s.area, 0)), 3) AS area,
                SUM(ifnull(s.r_approved,0)) r_approved, SUM(ifnull(s.d_approved,0)) d_approved,
                va.subdistrict_code AS subdistrict_code,
                rb.BlockNameHin AS subdistrict_name_hi,
                rb.BlockNameEng AS subdistrict_name_en,
                va.DistCodeCensus AS DistrictCode,
                rd.DistrictNameHindi AS district_name_hi,
                rd.DistrictName AS district_name_en
            FROM view_all_villages va
            INNER JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            INNER JOIN rev_block rb ON rb.subdistrict_code = va.subdistrict_code
            LEFT JOIN (
                SELECT COUNT(DISTINCT gs.application_id) AS f_count, COUNT(gs.sd_id) AS s_count, 
                    COUNT(if(fd.rheo_approval_status=1, fd.application_id, NULL)) r_approved,
                    COUNT(if(fd.ddh_approval_status=1, fd.application_id, NULL)) d_approved,
                    SUM(gs.area) AS area, gs.village_code 
                FROM temp_scheme_details gs
                INNER JOIN temp_farmer_application fd ON fd.application_id = gs.application_id
                WHERE gs.financial_year = @FinancialYear
                GROUP BY gs.village_code
            ) s ON s.village_code = va.village_code
            LEFT JOIN reg_target r ON r.user_id = rb.subdistrict_code AND r.financial_year = @FinancialYear
            WHERE va.DistCodeCensus = @DistrictCode
            GROUP BY va.subdistrict_code
            ORDER BY rb.BlockNameEng";

        var result = await connection.QueryAsync<BlockWiseFarmerApplicationsDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
        return Result<List<BlockWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<RHEOWiseFarmerApplicationsDto>>> GetRptOfRheoWiseApplicationsAsync(int departmentCode, int districtCode, int subDistrictCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                SUM(IFNULL(s.f_count, 0)) AS f_count,
                SUM(IFNULL(s.s_count, 0)) AS s_count,
                TRUNCATE(SUM(IFNULL(s.area, 0)), 3) AS area,
                ovd.village_code,
                ovd.officer_code,
                SUM(IFNULL(s.r_approved, 0)) r_approved,
                SUM(IFNULL(s.d_approved, 0)) d_approved,
                va.DistCodeCensus,
                va.DistrictName,
                va.subdistrict_code,
                va.subdistrict_name,
                m.name AS officer_name
            FROM view_all_villages va
            INNER JOIN officer_village_details ovd ON ovd.village_code = va.village_code AND ovd.department_code = @DepartmentCode
            INNER JOIN mas_raeo m ON m.officer_code = ovd.officer_code
            LEFT JOIN (
                SELECT 
                    COUNT(DISTINCT gs.application_id) AS f_count,
                    COUNT(gs.sd_id) AS s_count,
                    COUNT(IF(fd.rheo_approval_status = 1, fd.application_id, NULL)) r_approved,
                    COUNT(IF(fd.ddh_approval_status = 1, fd.application_id, NULL)) d_approved,
                    SUM(gs.area) AS area,
                    gs.village_code
                FROM temp_scheme_details gs
                INNER JOIN temp_farmer_application fd ON fd.application_id = gs.application_id
                WHERE gs.financial_year = @FinancialYear
                GROUP BY gs.village_code
            ) s ON s.village_code = ovd.village_code
            WHERE (@DistrictCode IS NULL OR @DistrictCode = 0 OR va.DistCodeCensus = @DistrictCode)
            AND (@SubDistrictCode IS NULL OR @SubDistrictCode = 0 OR va.subdistrict_code = @SubDistrictCode)
            GROUP BY ovd.officer_code
            ORDER BY m.name;";

        var result = await connection.QueryAsync<RHEOWiseFarmerApplicationsDto>(Sql, new { DepartmentCode = departmentCode, DistrictCode = districtCode, SubDistrictCode = subDistrictCode, FinancialYear = financialYear });
        return Result<List<RHEOWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<VillageWiseFarmerApplicationsDto>>> GetRptOfVillageWiseApplicationsAsync(int officerCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                SUM(IFNULL(s.f_count, 0)) AS f_count,
                SUM(IFNULL(s.s_count, 0)) AS s_count,
                TRUNCATE(SUM(IFNULL(s.area, 0)), 3) AS area,
                SUM(IFNULL(s.r_approved, 0)) r_approved,
                SUM(IFNULL(s.d_approved, 0)) d_approved,
                ovd.village_code,
                ovd.officer_code,
                va.DistCodeCensus,
                va.DistrictName,
                va.subdistrict_code,
                va.subdistrict_name,
                va.village_name,
                m.name AS officer_name,
                m.mobile_no
            FROM officer_village_details ovd
            INNER JOIN view_all_villages va ON ovd.village_code = va.village_code AND ovd.department_code = 3
            INNER JOIN mas_raeo m ON m.officer_code = ovd.officer_code
            LEFT JOIN (
                SELECT 
                    COUNT(DISTINCT gs.application_id) AS f_count,
                    COUNT(gs.sd_id) AS s_count,
                    COUNT(IF(fd.rheo_approval_status = 1, fd.application_id, NULL)) r_approved,
                    COUNT(IF(fd.ddh_approval_status = 1, fd.application_id, NULL)) d_approved,
                    SUM(gs.area) AS area,
                    gs.village_code
                FROM temp_scheme_details gs
                INNER JOIN temp_farmer_application fd ON fd.application_id = gs.application_id
                WHERE gs.financial_year = @FinancialYear
                GROUP BY gs.village_code
            ) s ON s.village_code = ovd.village_code
            WHERE ovd.officer_code = @OfficerCode
            GROUP BY ovd.village_code
            ORDER BY va.village_name;";

        var result = await connection.QueryAsync<VillageWiseFarmerApplicationsDto>(Sql, new { OfficerCode = officerCode, FinancialYear = financialYear });
        return Result<List<VillageWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<FarmerWiseFarmerApplicationsDto>>> GetRptOfFarmerWiseApplicationsAsync(
    int districtCode, int subDistrictCode, int villageCode, int officerCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
        SELECT DISTINCT
            fa.application_id, mf.uf_id, fa.fd_id, fa.hf_id, sd.sd_id, fa.village_code, v.village_name,
            mf.farmer_name_eng, mf.farmer_name_hi, mf.father_name, mf.mobile_no,
            sd.scheme_type, sd.scheme_id, ms.scheme_name, mc.cname AS component_name, sd.component_id,
            SUM(DISTINCT sd.area) AS area, SUM(DISTINCT sd.quantity) AS quantity,
            GROUP_CONCAT(DISTINCT ld.khasra_no, '(', ld.area, ') ') AS total_land_khasra,
            SUM(DISTINCT ld.area) AS total_land_area,
            GROUP_CONCAT(DISTINCT ' ', sk.khasra_no) AS khasra_no, sd.financial_year
        FROM temp_scheme_details sd
        INNER JOIN temp_farmer_application fa ON sd.application_id = fa.application_id
        INNER JOIN view_all_villages v ON v.village_code = fa.village_code
        INNER JOIN temp_mas_farmer mf ON mf.hf_id = sd.hf_id
        INNER JOIN mas_scheme_horti ms ON ms.s_id = sd.scheme_id
        INNER JOIN mas_component_horti mc ON mc.c_id = sd.component_id
        INNER JOIN temp_land_details ld ON ld.application_id = fa.application_id
        INNER JOIN temp_scheme_khasra_details sk ON sk.sd_id = sd.sd_id
        LEFT JOIN officer_village_details ovd ON ovd.village_code = fa.village_code
        LEFT JOIN mas_raeo mr ON mr.officer_code = ovd.officer_code AND mr.usertype = 3
        WHERE sd.financial_year = @FinancialYear
        AND (@DistrictCode IS NULL OR @DistrictCode = 0 OR v.DistCodeCensus = @DistrictCode)
        AND (@SubDistrictCode IS NULL OR @SubDistrictCode = 0 OR v.subdistrict_code = @SubDistrictCode)
        AND (@VillageCode IS NULL OR @VillageCode = 0 OR v.village_code = @VillageCode)
        AND (@OfficerCode IS NULL OR @OfficerCode = 0 OR mr.officer_code = @OfficerCode)
        GROUP BY sd.application_id, sd.scheme_id, sd.component_id
        ORDER BY v.village_name, mf.farmer_name_eng;";

        var result = await connection.QueryAsync<FarmerWiseFarmerApplicationsDto>(Sql, new
        {
            DistrictCode = districtCode,
            SubDistrictCode = subDistrictCode,
            VillageCode = villageCode,
            OfficerCode = officerCode,
            FinancialYear = financialYear
        });
        return Result<List<FarmerWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
}
