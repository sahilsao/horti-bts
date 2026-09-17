using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.BacklogYearly;

namespace HortiBts.Client.Services.Reports.BacklogYearly
{
    public class BacklogYearlyFarmerRegistrationApiService(HttpClient http)
    {
        public async Task<Result<List<DistWiseFarmerRegistrationDto>>> GetDistwiseFarmerRegistrationListAsync(int financialYear)
        {
            try
            {
                var response = await http.GetAsync("api/reports/backlog/distwise-farmer-registration?financialYear=" + financialYear);
                return await ApiResultHelper.ReadResultAsync<List<DistWiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistWiseFarmerRegistrationDto>>.Failure($"Failed to fetch district-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetBlockwiseFarmerRegistrationListAsync(int districtCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/backlog/blockwise-farmer-registration?districtCode={districtCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<BlockWiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockWiseFarmerRegistrationDto>>.Failure($"Failed to fetch block-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetRheoWiseFarmerRegistrationListAsync
           (int departmentCode, int districtCode, int subDistrictCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/backlog/rheowise-farmer-registration?" +
                    $"departmentCode={departmentCode}&districtCode={districtCode}&subDistrictCode={subDistrictCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<RheoWiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoWiseFarmerRegistrationDto>>.Failure($"Failed to fetch rheo-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<VillageWiseFarmerRegistrationDto>>> GetVillageWiseFarmerRegistrationListAsync
           (int officerCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/backlog/villagewise-farmer-registration?" +
                    $"officerCode={officerCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<VillageWiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillageWiseFarmerRegistrationDto>>.Failure($"Failed to fetch village-wise farmer registration report: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerWiseFarmerRegistrationDto>>> GetFarmerWiseFarmerRegistrationListAsync
           (int departmentCode,
             int districtCode,
             int subDistrictCode,
             int villageCode,
             int officerCode,
             int financialYear,
             int schemeTypeId,
             int schemeId,
             int componentId)
        {
            try
            {
                // for single year result here for farmer wise

                var response = await http.GetAsync(
                    $"api/reports/backlog/farmerwise-farmer-registration?" +
                    $"departmentCode={departmentCode}" +
                    $"&districtCode={districtCode}" +
                    $"&subDistrictCode={subDistrictCode}" +
                    $"&villageCode={villageCode}" +
                    $"&officerCode={officerCode}" +
                    $"&financialYear={financialYear}" +
                    $"&schemeTypeId={schemeTypeId}" +
                    $"&schemeId={schemeId}" +
                    $"&componentId={componentId}"
                );

                return await ApiResultHelper.ReadResultAsync<List<FarmerWiseFarmerRegistrationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerWiseFarmerRegistrationDto>>.Failure($"Failed to fetch farmer-wise farmer registration report: {ex.Message}");
            }
        }
    }
}
