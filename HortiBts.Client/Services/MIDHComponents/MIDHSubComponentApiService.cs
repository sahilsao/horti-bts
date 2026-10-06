using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.MIDHComponents;
using HortiBts.Shared.Dtos.MIDHSchemes;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.MIDHComponents
{
    public class MIDHSubComponentApiService(HttpClient http)
    {
        public async Task<Result<List<MIDHSubComponentDto>>> GetMIDHSubComponentsListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/midh-sub-components/midh-sub-components-list");
                return await ApiResultHelper.ReadResultAsync<List<MIDHSubComponentDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHSubComponentDto>>.Failure($"Failed to fetch midh sub components: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHSubComponentAsync(AddMIDHSubComponentDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/midh-sub-components/save-midh-sub-component-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(
                    $"Failed to save midh sub component : {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHSubComponentAsync(AddMIDHSubComponentDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/midh-sub-components/update-midh-sub-component-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update midh sub component : {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMIDHSubComponentActiveFlagAsync(int subComponentId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/midh-sub-components/{subComponentId}/active-flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update midh sub component active flag: {ex.Message}");
            }
        }
    }
}