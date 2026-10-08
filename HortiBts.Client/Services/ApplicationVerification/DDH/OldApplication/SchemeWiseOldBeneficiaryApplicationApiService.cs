using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH.OldApplication;

namespace HortiBts.Client.Services.ApplicationVerification.DDH.OldApplication;

public class SchemeWiseOldBeneficiaryApplicationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeWiseOldBeneficiaryApplicationListDto>>> GetSchemeWiseOldBeneficiaryApplicationAsync(int schemeId, int districtCode, int financialYear)
    {
        try
        {
            var response = await http.GetAsync("api/district/scheme-wise-old-beneficiaries-applications?schemeId=" + schemeId + "&districtCode=" + districtCode + "&financialYear=" + financialYear);
            return await ApiResultHelper.ReadResultAsync<List<SchemeWiseOldBeneficiaryApplicationListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeWiseOldBeneficiaryApplicationListDto>>.Failure($"Failed to fetch scheme-wise district applications: {ex.Message}");
        }
    }

}
