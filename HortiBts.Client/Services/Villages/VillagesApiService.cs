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
    }
}
