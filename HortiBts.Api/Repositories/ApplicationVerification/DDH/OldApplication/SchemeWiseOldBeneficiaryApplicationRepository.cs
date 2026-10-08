using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH.OldApplication;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.OldApplication;

public interface ISchemeWiseOldBeneficiaryApplicationRepository
{
    Task<Result<List<SchemeWiseOldBeneficiaryApplicationListDto>>> GetSchemeWiseOldBeneficiaryApplicationsAsync(int schemeId, int districtCode, int financialYear);
}

public class SchemeWiseOldBeneficiaryApplicationRepository(IDbConnectionFactory connectionFactory) : ISchemeWiseOldBeneficiaryApplicationRepository
{

    public async Task<Result<List<SchemeWiseOldBeneficiaryApplicationListDto>>> GetSchemeWiseOldBeneficiaryApplicationsAsync(int schemeId, int districtCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            WITH basic_detail AS(
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
                JOIN app_farmer_details fd ON v.village_code=fd.village_code
                INNER
                JOIN app_mas_farmer mf ON mf.hf_id=fd.hf_id
                WHERE v.DistCodeCensus= @DistrictCode ), bank_data AS(
                SELECT 
                    bd.ifsc_code,
                    bd.account_no,
                    bd.fd_id,
                    bd.bd_id
                FROM app_bank_details bd
                INNER
                JOIN basic_detail b ON b.fd_id=bd.fd_id
                AND bd.financial_year= @FinancialYear ), gov_scheme AS (
                SELECT 
                    gs.sd_id,
                    gs.scheme_type,
                    gs.scheme_id,
                    ms.scheme_name,
                    mc.cname,
                    gs.component_id,
                    gs.fd_id,
                    bd.village_code,
                    gs.khasra_no,
                    gs.area area,
                    gs.quantity quantity,
                    gs.financial_year,
                    gs.status,
                    gs.created_at reg_date
                FROM app_scheme_details gs
                INNER
                JOIN basic_detail bd ON bd.fd_id=gs.fd_id
                INNER
                JOIN mas_scheme_horti ms ON ms.s_id=gs.scheme_id
                INNER
                JOIN mas_component_horti mc ON mc.c_id=gs.component_id
                WHERE gs.financial_year= @FinancialYear
                AND gs.scheme_id= @SchemeId
                AND gs.status='R' ), scheme_khasra AS (
                SELECT 
                    gsk.sd_id,
                    gsk.fd_id,
                    gsk.uf_id,
                    gsk.village_code,
                    gsk.scheme_id,
                    gsk.component_id,
                    GROUP_CONCAT(' ', gsk.khasra_no) khasra_no
                FROM app_scheme_khasra_details gsk
                INNER
                JOIN basic_detail fd ON fd.fd_id=gsk.fd_id
                WHERE gsk.financial_year= @FinancialYear
                GROUP BY gsk.sd_id ), land_data AS(
                SELECT 
                    GROUP_CONCAT(ld.khasra_no,'(',ld.area,') ') AS total_land_khasra,
                    SUM(ld.area) total_land_area,
                    ld.fd_id
                FROM app_land_details ld
                INNER
                JOIN basic_detail bd ON bd.fd_id=ld.fd_id
                WHERE ld.financial_year= @FinancialYear
                GROUP BY ld.fd_id )
                SELECT 
                    gsc.sd_id,
                    bd.uf_id,
                    bd.fd_id,
                    bd.hf_id,
                    bd.village_code,
                    bd.village_name,
                    bd.farmer_name_eng,
                    bd.farmer_name_hi,
                    bd.father_name,
                    bd.mobile_no,
                    gsc.scheme_type,
                    gsc.scheme_id,
                    gsc.scheme_name,
                    gsc.cname component_name,
                    gsc.component_id,
                    gsc.area,
                    gsc.quantity,
                    gsc.financial_year,
                    sk.khasra_no,
                    ld1.total_land_khasra,
                    ld1.total_land_area,
                    gsc.status,
                    gsc.reg_date,
                    b.ifsc_code,
                    b.account_no
                FROM basic_detail bd
                INNER
                JOIN bank_data b ON b.fd_id=bd.fd_id
                INNER
                JOIN gov_scheme gsc ON bd.fd_id=gsc.fd_id
                INNER
                JOIN land_data ld1 ON ld1.fd_id=bd.fd_id
                LEFT
                JOIN scheme_khasra sk ON sk.sd_id=gsc.sd_id
                WHERE gsc.financial_year= @FinancialYear
                GROUP BY gsc.sd_id
                ORDER BY gsc.reg_date";

        var result = await connection.QueryAsync<SchemeWiseOldBeneficiaryApplicationListDto>(Sql, new { SchemeId = schemeId, DistrictCode = districtCode, FinancialYear = financialYear });
        return Result<List<SchemeWiseOldBeneficiaryApplicationListDto>>.Success(result.ToList());
    }
}
