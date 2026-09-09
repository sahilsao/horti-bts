using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Districts;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Districts
{
    public class DistrictsApiService(HttpClient http)
    {
        public async Task<Result<List<DistrictsDto>>> GetDistrictsAsync()
        {
            try
            {
                var response = await http.GetAsync("api/districts");
                return await ApiResultHelper.ReadResultAsync<List<DistrictsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistrictsDto>>.Failure($"Failed to fetch districts: {ex.Message}");
            }
        }
    }
}
