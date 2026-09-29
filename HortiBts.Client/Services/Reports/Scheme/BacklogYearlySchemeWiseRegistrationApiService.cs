using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.Scheme;

namespace HortiBts.Client.Services.Reports.Scheme
{
    public class BacklogYearlySchemeWiseRegistrationApiService(HttpClient http)
    {

        public async Task<Result<List<SchemeWiseRegistrationDto>>> GetSchemeWiseFarmerRegistrationListAsync
           (int districtCode,
             int subDistrictCode,
             int officerCode,             
             int villageCode,
             int financialYear)
        {
            try
            {
                // for single year result here for farmer wise

                var response = await http.GetAsync(
                    $"api/reports/backlog/scheme-wise-farmer-registration?" +
                    $"&districtCode={districtCode}" +
                    $"&subDistrictCode={subDistrictCode}" +
                    $"&officerCode={officerCode}" +                    
                    $"&villageCode={villageCode}" +
                    $"&financialYear={financialYear}"
                );

                return await ApiResultHelper.ReadResultAsync<List<SchemeWiseRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<SchemeWiseRegistrationDto>>.Failure($"Failed to fetch scheme-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
