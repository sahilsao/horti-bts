using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Client.Services.ApplicationVerification.DDH;

public class SchemeTypeWiseApplicationVerificationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeTypeWiseApplicationsListDto>>> GetSchemeTypeWiseDistrictApplicationsAsync(int schemeTypeId, int districtCode, int financialYear)
    {
        try
        {
            var response = await http.GetAsync("api/district/scheme-type-wise-applications?schemeTypeId=" + schemeTypeId + "&districtCode=" + districtCode + "&financialYear=" + financialYear);
            return await ApiResultHelper.ReadResultAsync<List<SchemeTypeWiseApplicationsListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeTypeWiseApplicationsListDto>>.Failure($"Failed to fetch scheme type wise district applications: {ex.Message}");
        }
    }

}
