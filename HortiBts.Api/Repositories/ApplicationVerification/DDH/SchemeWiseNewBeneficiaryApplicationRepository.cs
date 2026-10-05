using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH;

public interface ISchemeWiseNewBeneficiaryApplicationRepository
{
    Task<Result<List<SchemeWiseNewBeneficiaryApplicationListDto>>> GetSchemeWiseNewBeneficiaryApplicationsAsync(int schemeId, int districtCode, int financialYear);
}

public class SchemeWiseNewBeneficiaryApplicationRepository(IDbConnectionFactory connectionFactory) : ISchemeWiseNewBeneficiaryApplicationRepository
{

    public async Task<Result<List<SchemeWiseNewBeneficiaryApplicationListDto>>> GetSchemeWiseNewBeneficiaryApplicationsAsync(int schemeId, int districtCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT sd.application_id, sd.hf_id, sd.sd_id, mf.uf_id,mf.farmer_name_eng, mf.farmer_name_hi, mf.father_name, mf.mobile_no, 
            sd.scheme_type, sd.scheme_id, ms.scheme_name, mc.cname AS component_name, sd.component_id,
            sd.area, sd.quantity, sd.financial_year, sd.sd_id,  fa.hf_id, fa.village_code,
            b.ifsc_code, b.account_no, ld1.total_land_khasra, ld1.total_land_area, sk.khasra_no, 
            sd.status, sd.created_at as reg_date, v.village_code, v.village_name
            FROM temp_scheme_details sd
            INNER JOIN mas_scheme_horti ms ON ms.s_id = sd.scheme_id
            INNER JOIN mas_component_horti mc ON mc.c_id = sd.component_id
            INNER JOIN temp_farmer_application fa ON fa.application_id= sd.application_id
            INNER JOIN temp_mas_farmer mf ON mf.hf_id = sd.hf_id
            INNER JOIN temp_bank_details b ON b.fd_id = sd.fd_id AND b.financial_year = @FinancialYear 
            INNER JOIN (
                SELECT ld.application_id, GROUP_CONCAT(ld.khasra_no, '(', ld.area, ') ') AS total_land_khasra, 
                SUM(ld.area) AS total_land_area
                FROM temp_land_details ld
                WHERE ld.financial_year = @FinancialYear 
                GROUP BY ld.application_id
            ) ld1 ON ld1.application_id = fa.application_id
            LEFT JOIN (
                SELECT gsk.sd_id, GROUP_CONCAT(' ', gsk.khasra_no) AS khasra_no
                FROM temp_scheme_khasra_details gsk
                WHERE gsk.financial_year = @FinancialYear 
                GROUP BY gsk.sd_id
            ) sk ON sk.sd_id = sd.sd_id
            INNER JOIN view_all_villages v ON v.village_code=sd.village_code
            WHERE sd.scheme_id = @SchemeId AND v.DistCodeCensus= @DistrictCode and fa.rheo_approval_status=1 and fa.ddh_approval_status=0
            ORDER BY sd.created_at";

        var result = await connection.QueryAsync<SchemeWiseNewBeneficiaryApplicationListDto>(Sql, new { SchemeId = schemeId, DistrictCode = districtCode, FinancialYear = financialYear });
        return Result<List<SchemeWiseNewBeneficiaryApplicationListDto>>.Success(result.ToList());
    }
}
