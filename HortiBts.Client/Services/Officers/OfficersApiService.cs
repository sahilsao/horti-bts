using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Officers;
using HortiBts.Shared.Dtos.SubDistricts;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Officers
{
    public class OfficersApiService(HttpClient http)
    {
        public async Task<Result<List<RheoOfficersDto>>> GetRheoOfficersListFromSubdistrictCodeAsync(int departmentCode, int subDistrictCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officers-from-sub-district?departmentCode={departmentCode}&subDistrictCode={subDistrictCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch rheo officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficerMappedVillagesDto>>> GetRheoOfficerMappedVillagesListAsync(int departmentCode, int officerCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officer-mapped-villages?departmentCode={departmentCode}&officerCode={officerCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficerMappedVillagesDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficerMappedVillagesDto>>.Failure($"Failed to fetch mapped district of rheo officer: {ex.Message}");
            }
        }
    }
}
