using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.Comparative;

namespace HortiBts.Client.Services.Reports.Comparative
{
    public class ComparativeFarmerRegistrationApiService(HttpClient http)
    {
        public async Task<Result<List<DistwiseComparativeFarmerRegistrationDto>>> GetComparativeDistWiseFarmerRegistrationListAsync(int financialYear)
        {
            try
            {
                var response = await http.GetAsync("api/reports/comparative/distwise-farmer-registration?financialYear=" + financialYear);
                return await ApiResultHelper.ReadResultAsync<List<DistwiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistwiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch district-wise farmer registration report: {ex.Message}");
            }
        }       
    }
}
