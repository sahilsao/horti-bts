using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Units;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Units
{
    public class UnitsApiService(HttpClient http)
    {
        public async Task<Result<List<UnitDto>>> GetUnitsAsync()
        {
            try
            {
                var data = await http.GetFromJsonAsync<Result<List<UnitDto>>>($"api/units/get-all-units");
                return data ?? Result<List<UnitDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<UnitDto>>.Failure($"Failed to fetch units: {ex.Message}");
            }
        }
    }
}
