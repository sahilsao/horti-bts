using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH;

public interface ISchemeWiseFarmerVerificationRepository
{
    Task<Result<List<SchemeWiseApplicationsListDto>>> GetSchemeWiseDistrictApplicationsAsync(int financialYear, int districtCode, int schemeTypeId);
}

public class SchemeWiseFarmerVerificationRepository(IDbConnectionFactory connectionFactory) : ISchemeWiseFarmerVerificationRepository
{
    public async Task<Result<List<SchemeWiseApplicationsListDto>>> GetSchemeWiseDistrictApplicationsAsync(int financialYear, int districtCode, int schemeTypeId)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                s.s_id AS SchemeId,
                s.scheme_name AS SchemeName,
                s.st_id AS SchemeTypeId,
                COUNT(fa.application_id) AS total_applications,
                COUNT(CASE WHEN fa.rheo_approval_status <> 0 THEN fa.application_id END) AS rheo_approved,
                COUNT(CASE WHEN fa.ddh_approval_status <> 0 THEN fa.application_id END) AS ddh_approved
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

        var result = await connection.QueryAsync<SchemeWiseApplicationsListDto>(Sql, new { FinancialYear = financialYear, DistrictCode = districtCode, SchemeTypeId = schemeTypeId });
        return Result<List<SchemeWiseApplicationsListDto>>.Success(result.ToList());
    }
}
