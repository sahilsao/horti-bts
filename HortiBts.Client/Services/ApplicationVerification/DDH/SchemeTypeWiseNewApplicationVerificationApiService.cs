using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Client.Services.ApplicationVerification.DDH;

public class SchemeTypeWiseNewApplicationVerificationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeTypeWiseNewApplicationsListDto>>> GetSchemeTypeWiseNewDistrictApplicationsAsync(int schemeTypeId, int districtCode, int financialYear)
    {
        try
        {
            var response = await http.GetAsync("api/district/scheme-type-wise-new-applications?schemeTypeId=" + schemeTypeId + "&districtCode=" + districtCode + "&financialYear=" + financialYear);
            return await ApiResultHelper.ReadResultAsync<List<SchemeTypeWiseNewApplicationsListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeTypeWiseNewApplicationsListDto>>.Failure($"Failed to fetch scheme type wise district applications: {ex.Message}");
        }
    }

}
