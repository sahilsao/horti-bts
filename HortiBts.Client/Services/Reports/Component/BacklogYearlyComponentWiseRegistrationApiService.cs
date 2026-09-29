using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.Component;

namespace HortiBts.Client.Services.Reports.Component
{
    public class BacklogYearlyComponentWiseRegistrationApiService(HttpClient http)
    {

        public async Task<Result<List<ComponentWiseRegistrationDto>>> GetComponentWiseFarmerRegistrationListAsync
           (int districtCode,
             int subDistrictCode,
             int villageCode,
             int officerCode,
             int financialYear,
             int schemeId)
        {
            try
            {
                // for single year result here for farmer wise

                var response = await http.GetAsync(
                    $"api/reports/backlog/component-wise-farmer-registration?" +
                    $"&districtCode={districtCode}" +
                    $"&subDistrictCode={subDistrictCode}" +
                    $"&villageCode={villageCode}" +
                    $"&officerCode={officerCode}" +
                    $"&financialYear={financialYear}" +
                    $"&SchemeID={schemeId}"
                );

                return await ApiResultHelper.ReadResultAsync<List<ComponentWiseRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<ComponentWiseRegistrationDto>>.Failure($"Failed to fetch component-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
