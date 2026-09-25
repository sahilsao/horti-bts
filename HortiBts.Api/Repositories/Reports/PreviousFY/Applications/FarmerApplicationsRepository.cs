using System;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.PreviousFY.Applications;

namespace HortiBts.Api.Repositories.Reports.PreviousFY.Applications;

public interface IFarmerApplicationsRepository
{

    Task<Result<List<DistrictWiseFarmerApplicationsDto>>> GetRptOfDistrictWiseApplicationsAsync(int financialYear);
    Task<Result<List<BlockWiseFarmerApplicationsDto>>> GetRptOfBlockWiseApplicationsAsync(int districtCode, int financialYear);
    Task<Result<List<RHEOWiseFarmerApplicationsDto>>> GetRptOfRheoWiseApplicationsAsync(int departmentCode, int districtCode, int subDistrictCode, int financialYear);
    Task<Result<List<VillageWiseFarmerApplicationsDto>>> GetRptOfVillageWiseApplicationsAsync(int officerCode, int financialYear);
    Task<Result<List<FarmerWiseFarmerApplicationsDto>>> GetRptOfFarmerWiseApplicationsAsync(int districtCode, int subDistrictCode, int villageCode, int officerCode, int financialYear);
    Task<Result<List<ApprovedFarmerApplicationsDto>>> GetRptOfApprovedFarmersApplicationsAsync(int districtCode, int subDistrictCode, int villageCode, int officerCode, int schemeTypeId, int schemeId, char statusCode, int financialYear);

}
public class FarmerApplicationsRepository(IDbConnectionFactory connectionFactory) : IFarmerApplicationsRepository
{
    public async Task<Result<List<DistrictWiseFarmerApplicationsDto>>> GetRptOfDistrictWiseApplicationsAsync(int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code,
                v.DistCodeCensus,
                v.DistrictName
            FROM view_all_villages v ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM app_farmer_details fd
            INNER
            JOIN app_mas_farmer mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                count(DISTINCT gs.fd_id) f_count,
                COUNT( gs.sd_id) s_count,
                SUM(gs.area) area,
                fd.village_code
            FROM app_scheme_details gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year= @FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                IFNULL(r.target_count,0) AS target,
                sum( ifnull(s.f_count,0)) AS f_count,
                sum(ifnull(s.s_count,0)) AS s_count,
                truncate(sum(ifnull(s.area,0)),3) AS area,
                va.DistCodeCensus AS district_code,
                rd.DistrictNameHindi AS district_name_hi,
                rd.DistrictName AS district_name_en
            FROM villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus=va.DistCodeCensus
            LEFT
            JOIN scheme s ON s.village_code=va.village_code
            LEFT
            JOIN reg_target r ON r.user_id=rd.DistrictCensus
            AND r.financial_year= @FinancialYear
            GROUP BY va.DistCodeCensus
            ORDER BY va.DistrictName";

        var result = await connection.QueryAsync<DistrictWiseFarmerApplicationsDto>(Sql, new { FinancialYear = financialYear });
        return Result<List<DistrictWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<BlockWiseFarmerApplicationsDto>>> GetRptOfBlockWiseApplicationsAsync(int districtCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code,
                v.subdistrict_code,
                v.subdistrict_name,
                v.DistCodeCensus,
                v.DistrictName
            FROM view_all_villages v
            WHERE v.DistCodeCensus= @DistrictCode ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM app_farmer_details fd
            INNER
            JOIN app_mas_farmer mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                COUNT(DISTINCT gs.fd_id) f_count,
                COUNT(gs.sd_id) s_count,
                SUM(gs.area ) area,
                fd.village_code
            FROM app_scheme_details gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year= @FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                SUM(IFNULL(s.f_count,0)) AS f_count,
                SUM(IFNULL(s.s_count,0)) AS s_count,
                TRUNCATE(SUM(IFNULL(s.area,0)),3) AS AREA,
                va.subdistrict_code,
                rb.BlockNameHin AS subdistrict_name_hi,
                rb.BlockNameEng AS subdistrict_name_en,
                rd.DistrictNameHindi AS DistrictNameHi,
                rd.DistrictName AS DistrictNameEn
            FROM villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus=va.DistCodeCensus
            INNER
            JOIN rev_block rb ON rb.subdistrict_code=va.subdistrict_code
            LEFT
            JOIN scheme s ON s.village_code=va.village_code
            GROUP BY va.subdistrict_code
            ORDER BY rb.BlockNameEng";

        var result = await connection.QueryAsync<BlockWiseFarmerApplicationsDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
        return Result<List<BlockWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<RHEOWiseFarmerApplicationsDto>>> GetRptOfRheoWiseApplicationsAsync(int departmentCode, int districtCode, int subDistrictCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH villages AS (
            SELECT 
                ovd.village_code,
                ovd.officer_code,
                v.DistCodeCensus,
                v.DistrictName,
                v.subdistrict_code,
                v.subdistrict_name
            FROM officer_village_details ovd
            INNER
            JOIN view_all_villages v ON v.village_code = ovd.village_code
            WHERE (@DepartmentCode IS NULL
            OR @DepartmentCode = 0
            OR ovd.department_code = @DepartmentCode)
            AND (@DistrictCode IS NULL
            OR @DistrictCode = 0
            OR v.DistCodeCensus = @DistrictCode)
            AND (@SubDistrictCode IS NULL
            OR @SubDistrictCode = 0
            OR v.subdistrict_code = @SubDistrictCode) ), farmer_details AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM app_farmer_details fd
            INNER
            JOIN app_mas_farmer mf ON mf.hf_id = fd.hf_id
            INNER
            JOIN villages v ON v.village_code = fd.village_code ), schemes AS (
            SELECT 
                COUNT(DISTINCT gs.fd_id) AS f_count,
                COUNT(gs.sd_id) AS s_count,
                SUM(gs.area) AS area,
                fd.village_code
            FROM app_scheme_details gs
            INNER
            JOIN farmer_details fd ON fd.fd_id = gs.fd_id
            WHERE gs.financial_year = @FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                SUM(COALESCE(s.f_count, 0)) AS f_count,
                SUM(COALESCE(s.s_count, 0)) AS s_count,
                TRUNCATE(SUM(COALESCE(s.area, 0)), 3) AS area,
                va.officer_code,
                m.name AS officer_name,
                m.mobile_no,
                va.subdistrict_code,
                rb.BlockNameHin AS subdistrict_name_hi,
                rb.BlockNameEng AS subdistrict_name_en,
                va.DistCodeCensus AS district_code,
                rd.DistrictNameHindi AS district_name_hi,
                rd.DistrictName AS district_name_en
            FROM villages va
            INNER
            JOIN rev_block rb ON rb.subdistrict_code = va.subdistrict_code
            INNER
            JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            INNER
            JOIN mas_raeo m ON m.officer_code = va.officer_code
            LEFT
            JOIN schemes s ON s.village_code = va.village_code
            GROUP BY m.officer_code
            ORDER BY va.DistrictName, va.subdistrict_name, m.name;";

        var result = await connection.QueryAsync<RHEOWiseFarmerApplicationsDto>(Sql, new { DepartmentCode = departmentCode, DistrictCode = districtCode, SubDistrictCode = subDistrictCode, FinancialYear = financialYear });
        return Result<List<RHEOWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<VillageWiseFarmerApplicationsDto>>> GetRptOfVillageWiseApplicationsAsync(int officerCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH villages AS (
            SELECT 
                ovd.village_code,
                ovd.officer_code
            FROM officer_village_details ovd
            WHERE ovd.officer_code= @OfficerCode ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM app_farmer_details fd
            INNER
            JOIN app_mas_farmer mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                count(DISTINCT gs.fd_id) f_count,
                COUNT( gs.sd_id) s_count,
                SUM( gs.area) area,
                fd.village_code
            FROM app_scheme_details gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year= @FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                ifnull(s.f_count,0) AS f_count,
                ifnull(s.s_count,0) AS s_count,
                ifnull(s.area,0) AS area,
                ovd.village_code,
                va.village_name,
                ovd.officer_code,
                m.name AS officer_name,
                m.mobile_no
            FROM villages ovd
            INNER
            JOIN view_all_villages va ON ovd.village_code = va.village_code
            INNER
            JOIN mas_raeo m ON m.officer_code=ovd.officer_code
            LEFT
            JOIN scheme s ON s.village_code=ovd.village_code
            GROUP BY ovd.village_code
            ORDER BY va.village_name";

        var result = await connection.QueryAsync<VillageWiseFarmerApplicationsDto>(Sql, new { OfficerCode = officerCode, FinancialYear = financialYear });
        return Result<List<VillageWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<FarmerWiseFarmerApplicationsDto>>> GetRptOfFarmerWiseApplicationsAsync(int districtCode, int subDistrictCode, int villageCode, int officerCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH basic_detail AS (
            SELECT 
                mf.farmer_name_eng,
                mf.farmer_name_hi,
                mf.father_name,
                mf.mobile_no,
                mf.uf_id,
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code,
                va.village_name
            FROM officer_village_details v
            INNER
            JOIN view_all_villages va ON v.village_code = va.village_code
            INNER
            JOIN mas_raeo mr ON mr.officer_code = v.officer_code
            AND mr.usertype = 3
            INNER
            JOIN farmer_detail_horti fd ON v.village_code = fd.village_code
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
            WHERE @OfficerCode IS NOT NULL
            AND @OfficerCode <> 0
            AND v.officer_code = @OfficerCode
            AND (@VillageCode IS NULL
            OR @VillageCode = 0
            OR v.village_code = @VillageCode)
            UNION ALL
            SELECT 
                mf.farmer_name_eng,
                mf.farmer_name_hi,
                mf.father_name,
                mf.mobile_no,
                mf.uf_id,
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code,
                v.village_name
            FROM view_all_villages v
            INNER
            JOIN farmer_detail_horti fd ON v.village_code = fd.village_code
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
            WHERE (@OfficerCode IS NULL
            OR @OfficerCode = 0)
            AND (@DistrictCode IS NULL
            OR @DistrictCode = 0
            OR v.DistCodeCensus = @DistrictCode)
            AND (@SubDistrictCode IS NULL
            OR @SubDistrictCode = 0
            OR v.subdistrict_code = @SubDistrictCode)
            AND (@VillageCode IS NULL
            OR @VillageCode = 0
            OR v.village_code = @VillageCode) ), gov_scheme AS (
            SELECT 
                gs.scheme_type,
                gs.sd_id,
                gs.scheme_id,
                ms.scheme_name,
                mc.cname,
                gs.component_id,
                gs.fd_id,
                bd.village_code,
                gs.khasra_no,
                SUM(gs.area) AS area,
                SUM(gs.quantity) AS quantity,
                gs.financial_year
            FROM app_scheme_details gs
            INNER
            JOIN basic_detail bd ON bd.fd_id = gs.fd_id
            INNER
            JOIN mas_scheme_horti ms ON ms.s_id = gs.scheme_id
            INNER
            JOIN mas_component_horti mc ON mc.c_id = gs.component_id
            WHERE gs.financial_year = @FinancialYear
            GROUP BY gs.fd_id, gs.scheme_id, gs.component_id ), scheme_khasra AS (
            SELECT 
                gsk.fd_id,
                gsk.uf_id,
                gsk.village_code,
                gsk.scheme_id,
                gsk.component_id,
                GROUP_CONCAT(' ', gsk.khasra_no) AS khasra_no
            FROM app_scheme_khasra_details gsk
            INNER
            JOIN basic_detail bd ON bd.fd_id = gsk.fd_id
            WHERE gsk.financial_year = @FinancialYear
            GROUP BY gsk.fd_id, gsk.scheme_id, gsk.component_id ), land_data AS (
            SELECT 
                GROUP_CONCAT( ld.khasra_no, '(', ld.area, ') ' ) AS total_land_khasra,
                SUM(ld.area) AS total_land_area,
                ld.fd_id
            FROM app_land_details ld
            INNER
            JOIN basic_detail bd ON bd.fd_id = ld.fd_id
            WHERE ld.financial_year = @FinancialYear
            GROUP BY ld.fd_id )
            SELECT 
                bd.uf_id,
                bd.fd_id,
                bd.hf_id,
                gsc.sd_id,
                bd.village_code,
                bd.village_name,
                bd.farmer_name_eng,
                bd.farmer_name_hi,
                bd.father_name,
                bd.mobile_no,
                gsc.scheme_type,
                gsc.scheme_id,
                gsc.scheme_name,
                gsc.cname AS component_name,
                gsc.component_id,
                gsc.area,
                gsc.quantity,
                sk.khasra_no,
                ld1.total_land_khasra,
                ld1.total_land_area
            FROM basic_detail bd
            INNER
            JOIN gov_scheme gsc ON bd.fd_id = gsc.fd_id
            AND gsc.financial_year = @FinancialYear
            INNER
            JOIN land_data ld1 ON ld1.fd_id = bd.fd_id
            INNER
            JOIN scheme_khasra sk ON sk.fd_id = bd.fd_id
            GROUP BY bd.fd_id, gsc.scheme_id, gsc.component_id
            ORDER BY bd.village_name, bd.farmer_name_eng;";

        var result = await connection.QueryAsync<FarmerWiseFarmerApplicationsDto>(Sql, new { DistrictCode = districtCode, SubDistrictCode = subDistrictCode, VillageCode = villageCode, OfficerCode = officerCode, FinancialYear = financialYear });
        return Result<List<FarmerWiseFarmerApplicationsDto>>.Success(result.ToList());
    }
    public async Task<Result<List<ApprovedFarmerApplicationsDto>>> GetRptOfApprovedFarmersApplicationsAsync(int districtCode, int subDistrictCode, int villageCode, int officerCode, int schemeTypeId, int schemeId, char statusCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH basic_detail AS (
            SELECT
                mf.farmer_name_eng, mf.farmer_name_hi, mf.father_name, mf.mobile_no, mf.uf_id,
                fd.fd_id, fd.hf_id, fd.rheo_id, fd.village_code, v.village_name
            FROM view_all_villages v
            INNER JOIN app_farmer_details fd ON v.village_code = fd.village_code
            INNER JOIN app_mas_farmer mf ON mf.hf_id = fd.hf_id
            WHERE
                (
                    @OfficerCode <> 0
                    AND EXISTS (
                        SELECT 1 FROM officer_village_details ovd
                        WHERE ovd.village_code = v.village_code
                        AND ovd.department_code = 3
                        AND ovd.officer_code = @OfficerCode
                    )
                )
                OR (@OfficerCode = 0 AND @SubDistrictCode <> 0 AND v.subdistrict_code = @SubDistrictCode)
                OR (@OfficerCode = 0 AND @SubDistrictCode = 0 AND @DistrictCode <> 0 AND v.DistCodeCensus = @DistrictCode)
                OR (@OfficerCode = 0 AND @SubDistrictCode = 0 AND @DistrictCode = 0)
        ),
        gov_scheme AS (
            SELECT
                gs.sd_id, gs.scheme_type, gs.scheme_id, ms.scheme_name, mc.cname,
                gs.component_id, gs.fd_id, gs.khasra_no, gs.area, gs.quantity,
                gs.financial_year, gs.status, gs.created_at AS reg_date
            FROM app_scheme_details gs
            INNER JOIN mas_scheme_horti ms ON ms.s_id = gs.scheme_id
            INNER JOIN mas_component_horti mc ON mc.c_id = gs.component_id
            WHERE
                gs.status = @StatusCode
                AND gs.financial_year = @FinancialYear
                AND (@SchemeType = 0 OR gs.scheme_type = @SchemeType)
                AND (@SchemeId = 0 OR gs.scheme_id = @SchemeId)
        ),
        bank_data AS (
            SELECT bd.ifsc_code, bd.account_no, bd.fd_id
            FROM app_bank_details bd
            WHERE bd.financial_year = @FinancialYear
            GROUP BY bd.fd_id
        ),
        land_data AS (
            SELECT
                GROUP_CONCAT(ld.khasra_no, '(', ld.area, ') ') AS total_land_khasra,
                SUM(ld.area) AS total_land_area,
                ld.fd_id
            FROM app_land_details ld
            WHERE ld.financial_year = @FinancialYear
            GROUP BY ld.fd_id
        ),
        scheme_khasra AS (
            SELECT
                gsk.sd_id, gsk.fd_id, GROUP_CONCAT(' ', gsk.khasra_no) AS khasra_no
            FROM app_scheme_khasra_details gsk
            WHERE gsk.financial_year = @FinancialYear
            GROUP BY gsk.sd_id
        )
        SELECT
            gsc.sd_id, bd.uf_id, bd.fd_id, bd.hf_id, bd.village_code, bd.village_name,
            bd.farmer_name_eng, bd.farmer_name_hi, bd.father_name, bd.mobile_no,
            gsc.scheme_type, gsc.scheme_id, gsc.scheme_name, gsc.cname AS component_name,
            gsc.component_id, gsc.area, gsc.quantity, gsc.financial_year,
            sk.khasra_no, ld1.total_land_khasra, ld1.total_land_area,
            gsc.status, gsc.reg_date, b.ifsc_code, b.account_no
        FROM basic_detail bd
        INNER JOIN gov_scheme gsc ON bd.fd_id = gsc.fd_id
        INNER JOIN bank_data b ON b.fd_id = bd.fd_id
        INNER JOIN land_data ld1 ON ld1.fd_id = bd.fd_id
        LEFT JOIN scheme_khasra sk ON sk.sd_id = gsc.sd_id
        GROUP BY gsc.sd_id
        ORDER BY gsc.reg_date DESC";

        var result = await connection.QueryAsync<ApprovedFarmerApplicationsDto>(Sql, new
        {
            DistrictCode = districtCode,
            SubDistrictCode = subDistrictCode,
            VillageCode = villageCode,
            OfficerCode = officerCode,
            SchemeType = schemeTypeId,
            SchemeId = schemeId,
            StatusCode = statusCode,
            FinancialYear = financialYear
        });
        return Result<List<ApprovedFarmerApplicationsDto>>.Success(result.ToList());
    }
}
