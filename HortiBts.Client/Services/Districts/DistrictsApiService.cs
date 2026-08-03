using HortiBts.Shared.Common;
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
                var data = await http.GetFromJsonAsync<List<DistrictsDto>>("api/districts");
                return Result<List<DistrictsDto>>.Success(data ?? []);
            }
            catch (Exception ex)
            {
                return Result<List<DistrictsDto>>.Failure($"Failed to fetch districts: {ex.Message}");
            }
        }
    }
}
