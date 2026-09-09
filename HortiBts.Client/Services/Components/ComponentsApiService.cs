using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Components
{
    public class ComponentsApiService(HttpClient http)
    {
        public async Task<Result<List<ComponentDto>>> GetComponentsListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/components/components-list");
                return await ApiResultHelper.ReadResultAsync<List<ComponentDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<ComponentDto>>.Failure($"Failed to fetch components: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveComponentAsync(AddComponentDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/components/save-component-data", dto);

                return await ApiResultHelper.ReadResultAsync<int>(response);
            }

            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save component: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateComponentAsync(AddComponentDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/components/update-component-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update component: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateComponentActiveFlagAsync(int componentId, bool flag)
        {
            try
            {
                var response = await http.PostAsJsonAsync($"api/components/{componentId}/flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update component flag: {ex.Message}");
            }
        }
    }
}