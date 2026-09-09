using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Farmers;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Farmers
{
    public class FarmersVerificationForUFPApiService(HttpClient http)
    {
        public async Task<Result<List<FarmersListByVillageForUFPVerificationDto>>> GetFarmersListFromVillageForUFPAsync(string VillageCode)
        {
            try
            {
                var response =
                    await http.GetAsync($"api/farmer-verification/get-farmers-list-by-village-from-ufp" +
                    $"?villageCode={Uri.EscapeDataString(VillageCode)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmersListByVillageForUFPVerificationDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmersListByVillageForUFPVerificationDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }
    }
}
