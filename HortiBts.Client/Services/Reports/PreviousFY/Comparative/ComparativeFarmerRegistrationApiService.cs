using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.PreviousFY.Comparative;

namespace HortiBts.Client.Services.Reports.PreviousFY.Comparative
{
    public class ComparativeFarmerRegistrationApiService(HttpClient http)
    {
        public async Task<Result<List<DistWiseComparativeFarmerRegistrationDto>>> GetComparativeDistWiseFarmerRegistrationListAsync(int financialYear)
        {
            try
            {
                var response = await http.GetAsync("api/reports/comparative/previous/distwise-farmer-registration?financialYear=" + financialYear);
                return await ApiResultHelper.ReadResultAsync<List<DistWiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistWiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch district-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<BlockWiseComparativeFarmerRegistrationDto>>> GetComparativeBlockWiseFarmerRegistrationListAsync(int districtCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/comparative/previous/blockwise-farmer-registration?districtCode={districtCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<BlockWiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockWiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch block-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoWiseComparativeFarmerRegistrationDto>>> GetComparativeRheoWiseFarmerRegistrationListAsync
           (int departmentCode, int districtCode, int subDistrictCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/comparative/previous/rheowise-farmer-registration?" +
                    $"departmentCode={departmentCode}&districtCode={districtCode}&subDistrictCode={subDistrictCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<RheoWiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoWiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch rheo-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<VillageWiseComparativeFarmerRegistrationDto>>> GetComparativeVillageWiseFarmerRegistrationListAsync
           (int officerCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/comparative/previous/villagewise-farmer-registration?" +
                    $"officerCode={officerCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<VillageWiseComparativeFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillageWiseComparativeFarmerRegistrationDto>>.Failure($"Failed to fetch village-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
