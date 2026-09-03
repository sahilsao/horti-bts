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
                var result = await http.GetFromJsonAsync<List<TargetDto>>("api/target/get-user-targets-list");
                return Result<List<TargetDto>>.Success(result ?? []);
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

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return Result<int>.Failure(
                        $"Failed to save component: {response.StatusCode} - {errorBody}");
                }

                var componentId = await response.Content.ReadFromJsonAsync<int>();
                return Result<int>.Success(componentId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(
                    $"Failed to save component: {ex.Message}");
            }
        }
    }
}
