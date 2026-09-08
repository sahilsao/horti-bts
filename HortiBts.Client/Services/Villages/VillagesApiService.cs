using HortiBts.Shared.Common;
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
                var data = await http.GetFromJsonAsync<Result<List<VillagesDto>>>($"api/get-villages-from-sub-district?subdistrictCode={subdistrictCode}");
                return data ?? Result<List<VillagesDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<VillagesDto>>.Failure($"Failed to fetch villages: {ex.Message}");
            }
        }
    }
}
