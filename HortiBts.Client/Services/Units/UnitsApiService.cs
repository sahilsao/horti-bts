using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Target;
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
                var response = await http.GetAsync("api/units/get-all-units");
                return await ApiResultHelper.ReadResultAsync<List<UnitDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<UnitDto>>.Failure($"Failed to fetch units: {ex.Message}");
            }
        }
    }
}
