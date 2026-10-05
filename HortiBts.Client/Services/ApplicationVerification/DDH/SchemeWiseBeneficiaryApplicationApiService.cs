using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Client.Services.ApplicationVerification.DDH;

public class SchemeWiseBeneficiaryApplicationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeWiseBeneficiaryApplicationListDto>>> GetSchemeWiseBeneficiaryApplicationAsync(int schemeId, int districtCode, int financialYear)
    {
        try
        {
            var response = await http.GetAsync("api/district/scheme-wise-beneficiaries-applications?schemeId=" + schemeId + "&districtCode=" + districtCode + "&financialYear=" + financialYear);
            return await ApiResultHelper.ReadResultAsync<List<SchemeWiseBeneficiaryApplicationListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeWiseBeneficiaryApplicationListDto>>.Failure($"Failed to fetch scheme-wise district applications: {ex.Message}");
        }
    }

}
