using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Target;
using HortiBts.Shared.Dtos.Villages;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Villages
{
    public class VillagesApiService(HttpClient http)
    {
        public async Task<Result<List<VillagesDto>>> GetVillagesAsync(int subdistrictCode)
        {
            try
            {
                var response = await http.GetAsync($"api/get-villages-from-sub-district?subdistrictCode={subdistrictCode}");
                return await ApiResultHelper.ReadResultAsync<List<VillagesDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillagesDto>>.Failure($"Failed to fetch villages: {ex.Message}");
            }
        }

        public async Task<Result<List<VillagesForRHEODto>>> GetVillagesForRheoAsync(int departmentCode, int subdistrictCode)
        {
            try
            {
                var response = await http.GetAsync($"api/get-villages-from-sub-district/for-rheo?DepartmentCode={departmentCode}&SubdistrictCode={subdistrictCode}");
                return await ApiResultHelper.ReadResultAsync<List<VillagesForRHEODto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillagesForRHEODto>>.Failure($"Failed to fetch villages for RHEO: {ex.Message}");
            }
        }
    }
}
