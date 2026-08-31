using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Schemes
{
    public class SchemesApiService(HttpClient http)
    {
        public async Task<Result<List<SchemeTypeDto>>> GetSchemesTypesAsync()
        {
            try
            {
                var result = await http.GetFromJsonAsync<List<SchemeTypeDto>>("api/schemes/schemes-types");
                return Result<List<SchemeTypeDto>>.Success(result ?? []);
            }
            catch (Exception ex)
            {
                return Result<List<SchemeTypeDto>>.Failure($"Failed to fetch scheme types: {ex.Message}");
            }
        }

        public async Task<Result<List<SchemeDto>>> GetSchemesListAsync()
        {
            try
            {
                var result = await http.GetFromJsonAsync<List<SchemeDto>>("api/schemes/schemes-list");
                return Result<List<SchemeDto>>.Success(result ?? []);
            }
            catch (Exception ex)
            {
                return Result<List<SchemeDto>>.Failure($"Failed to fetch schemes: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveSchemeAsync(AddSchemeDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = BuildMultipartContent(dto, file);

                var response = await http.PostAsync("api/schemes/save-scheme-data", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return Result<int>.Failure($"Failed to save scheme: {response.StatusCode} - {errorBody}");
                }

                var schemeId = await response.Content.ReadFromJsonAsync<int>();
                return Result<int>.Success(schemeId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save scheme: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateSchemeAsync(AddSchemeDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = BuildMultipartContent(dto, file);

                var response = await http.PostAsync("api/schemes/update-scheme-data", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return Result<int>.Failure($"Failed to update scheme: {response.StatusCode} - {errorBody}");
                }

                var schemeId = await response.Content.ReadFromJsonAsync<int>();
                return Result<int>.Success(schemeId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update scheme: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateSchemeBeneficiaryFlagAsync(int schemeId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/schemes/{schemeId}/beneficiary-flag", new { Flag = flag });

                if (!response.IsSuccessStatusCode)
                    return Result<bool>.Failure($"Failed to update beneficiary flag: {response.StatusCode}");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update beneficiary flag: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateSchemeActiveFlagAsync(int schemeId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/schemes/{schemeId}/active-flag", new { Flag = flag });

                if (!response.IsSuccessStatusCode)
                    return Result<bool>.Failure($"Failed to update active flag: {response.StatusCode}");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update active flag: {ex.Message}");
            }
        }

        private static MultipartFormDataContent BuildMultipartContent(AddSchemeDto dto, IBrowserFile? file)
        {
            var content = new MultipartFormDataContent();

            if (dto.SchemeId is not null)
                content.Add(new StringContent(dto.SchemeId.Value.ToString()), nameof(dto.SchemeId));

            content.Add(new StringContent(dto.SchemeTypeId!.ToString()), nameof(dto.SchemeTypeId));
            content.Add(new StringContent(dto.SchemeName ?? string.Empty), nameof(dto.SchemeName));
            content.Add(new StringContent(dto.SchemeNameEn ?? string.Empty), nameof(dto.SchemeNameEn));
            content.Add(new StringContent(dto.SchemeDescriptionHi ?? string.Empty), nameof(dto.SchemeDescriptionHi));
            content.Add(new StringContent(dto.SchemeDescriptionEn ?? string.Empty), nameof(dto.SchemeDescriptionEn));

            if (file is not null)
            {
                var streamContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024));
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "file", file.Name);
            }

            return content;
        }
    }
}