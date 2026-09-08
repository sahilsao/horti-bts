using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.SubDistricts;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.SubDistricts
{
    public class SubdistrictsApiService(HttpClient http)
    {
        public async Task<Result<List<SubDistrictsDto>>> GetSubdistrictsAsync(int districtCode)
        {
            try
            {
                var data = await http.GetFromJsonAsync<Result<List<SubDistrictsDto>>>($"api/get-sub-districts-from-district?districtCode={districtCode}");
                return data ?? Result<List<SubDistrictsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<SubDistrictsDto>>.Failure($"Failed to fetch subdistricts: {ex.Message}");
            }
        }
    }
}
