using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Target;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Target
{
    public class TargetApiService(HttpClient http)
    {
        public async Task<Result<List<TargetDto>>> GetTargetsListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/target/get-user-targets-list");
                return await ApiResultHelper.ReadResultAsync<List<TargetDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<TargetDto>>.Failure($"Failed to fetch targets: {ex.Message}");
            }
        }               

        public async Task<Result<int>> SaveTargetAsync(AddTargetDto Dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/target/save-target-data", Dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(
                    $"Failed to save component: {ex.Message}");
            }
        }
    }
}
