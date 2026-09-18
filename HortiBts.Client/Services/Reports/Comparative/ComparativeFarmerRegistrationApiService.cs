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

        public async Task<Result<List<BlockwiseComparativeFarmerRegistrationDto>>> GetComparativeBlockWiseFarmerRegistrationListAsync(int districtCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/comparative/blockwise-farmer-registration?districtCode={districtCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<BlockwiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockwiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch block-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<RheowiseComparativeFarmerRegistrationDto>>> GetComparativeRheoWiseFarmerRegistrationListAsync
           (int departmentCode, int districtCode, int subDistrictCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/comparative/rheowise-farmer-registration?" +
                    $"departmentCode={departmentCode}&districtCode={districtCode}&subDistrictCode={subDistrictCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<RheowiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheowiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch rheo-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<VillagewiseComparativeFarmerRegistrationDto>>> GetComparativeVillageWiseFarmerRegistrationListAsync
           (int officerCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/comparative/villagewise-farmer-registration?" +
                    $"officerCode={officerCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<VillagewiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillagewiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch village-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
