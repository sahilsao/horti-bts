using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Client.Services.ApplicationVerification.DDH;

public class SchemeTypeWiseOldApplicationVerificationApiService(HttpClient http)
{
    public async Task<Result<List<SchemeTypeWiseOldApplicationsListDto>>> GetSchemeTypeWiseOldApplicationsAsync(
        int districtCode,
        int subDistrictCode,
        int villageCode,
        int officerCode,
        int schemeTypeId,
        int financialYear)
    {

        try
        {
            var url = $"api/district/scheme-type-wise-old-applications" +
                       $"?districtCode={districtCode}" +
                       $"&subDistrictCode={subDistrictCode}" +
                       $"&villageCode={villageCode}" +
                       $"&officerCode={officerCode}" +
                       $"&schemeTypeId={schemeTypeId}" +
                       $"&financialYear={financialYear}";

            var response = await http.GetAsync(url);
            return await ApiResultHelper.ReadResultAsync<List<SchemeTypeWiseOldApplicationsListDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<SchemeTypeWiseOldApplicationsListDto>>.Failure($"Failed to fetch farmer list: {ex.Message}");
        }
    }

}

public class SchemeTypeWiseOldApplicationsQueryParams
{
    public string SearchFlag { get; set; } = string.Empty; // DIST, SUBDIST, OFFICER, VILL, FARMER
    public int FinancialYear { get; set; }
    public int? Id { get; set; }
}
