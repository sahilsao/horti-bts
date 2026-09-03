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
                var data = await http.GetFromJsonAsync<List<UnitDto>>($"api/units/get-all-units");
                return Result<List<UnitDto>>.Success(data ?? []);
            }
            catch (Exception ex)
            {
                return Result<List<UnitDto>>.Failure($"Failed to fetch subdistricts: {ex.Message}");
            }
        }
    }
}
