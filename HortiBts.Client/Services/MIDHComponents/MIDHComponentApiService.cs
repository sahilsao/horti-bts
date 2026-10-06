using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.MIDHComponents;
using HortiBts.Shared.Dtos.MIDHSchemes;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.MIDHComponents
{
    public class MIDHComponentApiService(HttpClient http)
    {
        public async Task<Result<List<MIDHComponentDto>>> GetMIDHComponentsListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/midh-components/midh-components-list");
                return await ApiResultHelper.ReadResultAsync<List<MIDHComponentDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentDto>>.Failure($"Failed to fetch midh components: {ex.Message}");
            }
        }

        public async Task<Result<List<MIDHComponentDto>>> GetMIDHComponentsListByComponentIdAsync(int componentId)
        {
            try
            {
                var response = await http.GetAsync("api/midh-components/midh-components-list-by-component-id?componentId=" + componentId);
                return await ApiResultHelper.ReadResultAsync<List<MIDHComponentDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentDto>>.Failure($"Failed to fetch midh components: {ex.Message}");
            }
        }

        public async Task<Result<List<MIDHComponentDto>>> GetMIDHComponentsListByComponentTypeIdAsync(int componentTypeId)
        {
            try
            {
                var response = await http.GetAsync("api/midh-components/midh-components-list-by-component-type-id?componentTypeId=" + componentTypeId);
                return await ApiResultHelper.ReadResultAsync<List<MIDHComponentDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentDto>>.Failure($"Failed to fetch midh components: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHComponentAsync(AddMIDHComponentDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/midh-components/save-midh-component-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(
                    $"Failed to save midh component : {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHComponentAsync(AddMIDHComponentDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/midh-components/update-midh-component-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update midh component : {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMIDHComponentActiveFlagAsync(int componentId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/midh-components/{componentId}/active-flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update midh component active flag: {ex.Message}");
            }
        }
    }
}