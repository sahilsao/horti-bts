using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.SubDistricts;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.SubDistricts
{
    public class SubDistrictsApiService(HttpClient http)
    {
        public async Task<Result<List<SubDistrictsDto>>> GetSubDistrictsAsync(int districtCode)
        {
            try
            {
                var response = await http.GetAsync($"api/get-sub-districts-from-district?districtCode={districtCode}");
                return await ApiResultHelper.ReadResultAsync<List<SubDistrictsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<SubDistrictsDto>>.Failure($"Failed to fetch subdistricts: {ex.Message}");
            }
        }
    }
}
