using HortiBts.Client.Pages.Farmers.FarmerDetails.Models;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.MIDHComponents;
using HortiBts.Shared.Dtos.MIDHSchemes;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.MIDHComponents
{
    public class MIDHComponentTypeApiService(HttpClient http)
    {
        public async Task<Result<List<MIDHComponentTypeDto>>> GetMIDHComponentTypesListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/midh-components-types/midh-components-type-list");
                return await ApiResultHelper.ReadResultAsync<List<MIDHComponentTypeDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentTypeDto>>.Failure($"Failed to fetch midh component types: {ex.Message}");
            }
        }
        public async Task<Result<List<MIDHComponentTypeDto>>> GetComponentTypesListBySchemeIdAsync(int schemeId)
        {
            try
            {
                var response = await http.GetAsync("api/midh-components-types/midh-components-type-list-by-scheme-id?schemeId=" + schemeId);
                return await ApiResultHelper.ReadResultAsync<List<MIDHComponentTypeDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHComponentTypeDto>>.Failure($"Failed to fetch midh component types: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHComponentTypeAsync(AddMIDHComponentTypeDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/midh-components-types/save-midh-component-type-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(
                    $"Failed to save midh component type: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHComponentTypeAsync(AddMIDHComponentTypeDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/midh-components-types/update-midh-component-type-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update midh component type: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMIDHComponentTypeActiveFlagAsync(int componentTypeId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/midh-components-types/{componentTypeId}/active-flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update midh component type active flag: {ex.Message}");
            }
        }
    }
}