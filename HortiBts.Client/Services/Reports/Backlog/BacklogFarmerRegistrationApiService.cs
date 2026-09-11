using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports;

namespace HortiBts.Client.Services.Reports.Backlog
{
    public class BacklogFarmerRegistrationApiService(HttpClient http)
    {
        public async Task<Result<List<DistwiseFarmerRegistrationDto>>> GetDistwiseFarmerRegistrationListAsync(int financialYear)
        {
            try
            {
                var response = await http.GetAsync("api/reports/backlog/distwise-farmer-registration?financialYear=" + financialYear);
                return await ApiResultHelper.ReadResultAsync<List<DistwiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistwiseFarmerRegistrationDto>>.Failure($"Failed to fetch district-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<BlockwiseFarmerRegistrationDto>>> GetBlockwiseFarmerRegistrationListAsync(int districtCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/backlog/blockwise-farmer-registration?districtCode={districtCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<BlockwiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockwiseFarmerRegistrationDto>>.Failure($"Failed to fetch block-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
