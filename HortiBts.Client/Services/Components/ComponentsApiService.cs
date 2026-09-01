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
                var result = await http.GetFromJsonAsync<List<ComponentDto>>("api/components/components-list");
                return Result<List<ComponentDto>>.Success(result ?? []);
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
                var response = await http.PostAsJsonAsync("api/components/save-component-data", new
                {
                    dto.SchemeId,
                    dto.ComponentId,    
                    dto.ComponentNameEn,
                    dto.ComponentNameHi,
                    dto.ComponentDescriptionEn,
                    dto.ComponentDescriptionHi
                });

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return Result<int>.Failure($"Failed to save component: {response.StatusCode} - {errorBody}");
                }

                var componentId = await response.Content.ReadFromJsonAsync<int>();
                return Result<int>.Success(componentId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save component: {ex.Message}");
            }
        }

    }
}