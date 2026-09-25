using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;

namespace HortiBts.Client.Services.Reports.CurrentFY.Applications
{
    public class NewFarmerApplicationsListApiService(HttpClient http)
    {

        public async Task<Result<List<DistrictWiseFarmerApplicationsDto>>> GetRptOfDistrictWiseFarmerApplicationsListAsync(int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/applications/current/districtwise-list?financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<DistrictWiseFarmerApplicationsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistrictWiseFarmerApplicationsDto>>.Failure($"Failed to fetch district-wise farmer applications list: {ex.Message}");
            }
        }

        public async Task<Result<List<BlockWiseFarmerApplicationsDto>>> GetRptOfBlockWiseFarmerApplicationsListAsync(int districtCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/applications/current/blockwise-list?districtCode={districtCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<BlockWiseFarmerApplicationsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockWiseFarmerApplicationsDto>>.Failure($"Failed to fetch block-wise farmer applications list: {ex.Message}");
            }
        }

        public async Task<Result<List<RHEOWiseFarmerApplicationsDto>>> GetRptOfRheoWiseFarmerApplicationsListAsync(int departmentCode, int districtCode, int subDistrictCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/applications/current/rheowise-list?departmentCode={departmentCode}&districtCode={districtCode}&subDistrictCode={subDistrictCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<RHEOWiseFarmerApplicationsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RHEOWiseFarmerApplicationsDto>>.Failure($"Failed to fetch rheo-wise farmer applications list: {ex.Message}");
            }
        }

        public async Task<Result<List<VillageWiseFarmerApplicationsDto>>> GetRptOfVillageWiseFarmerApplicationsListAsync(int officerCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/applications/current/villagewise-list?officerCode={officerCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<VillageWiseFarmerApplicationsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillageWiseFarmerApplicationsDto>>.Failure($"Failed to fetch rheo-wise farmer applications list: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerWiseFarmerApplicationsDto>>> GetRptOfFarmerWiseFarmerApplicationsListAsync(int districtCode, int subDistrictCode, int villageCode, int officerCode, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/applications/current/farmerwise-list?districtCode={districtCode}&subDistrictCode={subDistrictCode}&villageCode={villageCode}&officerCode={officerCode}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerWiseFarmerApplicationsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerWiseFarmerApplicationsDto>>.Failure($"Failed to fetch farmer-wise farmer applications list: {ex.Message}");
            }
        }       
    }
}
