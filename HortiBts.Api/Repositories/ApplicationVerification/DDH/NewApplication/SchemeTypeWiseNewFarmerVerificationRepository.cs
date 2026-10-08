using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH.NewApplication;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.NewApplication;

public interface ISchemeTypeWiseNewFarmerVerificationRepository
{
    Task<Result<List<SchemeTypeWiseNewApplicationsListDto>>> GetSchemeTypeWiseNewDistrictApplicationsAsync(int schemeTypeId, int districtCode, int financialYear);
}

public class SchemeTypeWiseNewFarmerVerificationRepository(IDbConnectionFactory connectionFactory) : ISchemeTypeWiseNewFarmerVerificationRepository
{
    public async Task<Result<List<SchemeTypeWiseNewApplicationsListDto>>> GetSchemeTypeWiseNewDistrictApplicationsAsync(int schemeTypeId, int districtCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                s.s_id AS SchemeId,
                s.scheme_name AS SchemeName,
                s.st_id AS SchemeTypeId,
                COUNT(fa.application_id) AS total_applications,
                SUM(CASE WHEN fa.rheo_approval_status = 0 THEN 1 ELSE 0 END) AS rheo_pending,
                SUM(CASE WHEN fa.rheo_approval_status = 1 THEN 1 ELSE 0 END) AS rheo_approved,
                SUM(CASE WHEN fa.rheo_approval_status = 2 THEN 1 ELSE 0 END) AS rheo_rejected,
                SUM(CASE WHEN fa.rheo_approval_status = 1 AND fa.ddh_approval_status = 0 THEN 1 ELSE 0 END) AS ddh_pending,
                SUM(CASE WHEN fa.ddh_approval_status = 1 THEN 1 ELSE 0 END) AS ddh_approved,
                SUM(CASE WHEN fa.ddh_approval_status = 2 THEN 1 ELSE 0 END) AS ddh_rejected
            FROM mas_scheme_horti s
            LEFT
            JOIN (
            SELECT 
                sd.scheme_id,
                sd.sd_id,
                sd.application_id
            FROM temp_scheme_details sd
            INNER
            JOIN view_all_villages v ON v.village_code = sd.village_code
            AND v.DistCodeCensus= @DistrictCode) sd ON s.s_id = sd.scheme_id
            LEFT
            JOIN temp_farmer_application fa ON sd.application_id = fa.application_id
            AND fa.financial_year = @FinancialYear
            WHERE s.isbeneficiary = 'Y'
            AND s.flag = 'Y'
            AND s.st_id = @SchemeTypeId
            GROUP BY s.s_id
            ORDER BY s.scheme_name;";

        var result = await connection.QueryAsync<SchemeTypeWiseNewApplicationsListDto>(Sql, new { SchemeTypeId = schemeTypeId, DistrictCode = districtCode, FinancialYear = financialYear });
        return Result<List<SchemeTypeWiseNewApplicationsListDto>>.Success(result.ToList());
    }
}
