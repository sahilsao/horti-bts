using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports;

namespace HortiBts.Client.Services.Reports.Yearly
{
    public class YearlyFarmerRegistrationApiService(HttpClient http)
    {
        public async Task<Result<List<DistwiseFarmerRegistrationDto>>> GetDistwiseFarmerRegistrationListAsync(int financialYear)
        {
            try
            {
                var response = await http.GetAsync("api/reports/yearly/distwise-farmer-registration?financialYear=" + financialYear);
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
                var response = await http.GetAsync($"api/reports/yearly/blockwise-farmer-registration?districtCode={districtCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<BlockwiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockwiseFarmerRegistrationDto>>.Failure($"Failed to fetch block-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetRheowiseFarmerRegistrationListAsync
            (int departmentCode, int districtCode, int subDistrictCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/yearly/rheowise-farmer-registration?" +
                    $"departmentCode={departmentCode}&districtCode={districtCode}&subDistrictCode={subDistrictCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<RheoWiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoWiseFarmerRegistrationDto>>.Failure($"Failed to fetch rheo-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
