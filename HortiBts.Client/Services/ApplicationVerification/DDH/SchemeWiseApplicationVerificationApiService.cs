using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Client.Services.ApplicationVerification.DDH;

public class SchemeWiseApplicationVerificationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeWiseApplicationsListDto>>> GetSchemeWiseDistrictApplicationsAsync(int financialYear, int districtCode, int schemeTypeId)
    {
        try
        {
            var response = await http.GetAsync("api/district/scheme-wise-applications?financialYear=" + financialYear + "&districtCode=" + districtCode + "&schemeTypeId=" + schemeTypeId);
            return await ApiResultHelper.ReadResultAsync<List<SchemeWiseApplicationsListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeWiseApplicationsListDto>>.Failure($"Failed to fetch scheme-wise district applications: {ex.Message}");
        }
    }

}
