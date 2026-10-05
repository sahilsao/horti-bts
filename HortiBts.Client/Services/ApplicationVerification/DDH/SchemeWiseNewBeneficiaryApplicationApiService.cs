using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Client.Services.ApplicationVerification.DDH;

public class SchemeWiseNewBeneficiaryApplicationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeWiseNewBeneficiaryApplicationListDto>>> GetSchemeWiseNewBeneficiaryApplicationAsync(int schemeId, int districtCode, int financialYear)
    {
        try
        {
            var response = await http.GetAsync("api/district/scheme-wise-new-beneficiaries-applications?schemeId=" + schemeId + "&districtCode=" + districtCode + "&financialYear=" + financialYear);
            return await ApiResultHelper.ReadResultAsync<List<SchemeWiseNewBeneficiaryApplicationListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeWiseNewBeneficiaryApplicationListDto>>.Failure($"Failed to fetch scheme-wise district applications: {ex.Message}");
        }
    }

}
