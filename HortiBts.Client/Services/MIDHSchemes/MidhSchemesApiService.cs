using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.MIDHSchemes;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.MIDHSchemes
{
    public class MidhSchemesApiService(HttpClient http)
    {
        public async Task<Result<List<MIDHSchemeDto>>> GetMIDHSchemesListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/midh-schemes/midh-schemes-list");
                return await ApiResultHelper.ReadResultAsync<List<MIDHSchemeDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHSchemeDto>>.Failure($"Failed to fetch midh schemes: {ex.Message}");
            }
        }

        public async Task<Result<List<MIDHSchemeDto>>> GetMIDHSchemesListBySchemeTypeAsync(int schemeTypeId)
        {
            try
            {
                var response = await http.GetAsync("api/midh-schemes/midh-schemes-list-by-scheme-id?schemeTypeId=" + schemeTypeId);
                return await ApiResultHelper.ReadResultAsync<List<MIDHSchemeDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<MIDHSchemeDto>>.Failure($"Failed to fetch midh schemes: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveMIDHSchemeAsync(AddMIDHSchemeDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = BuildMultipartContent(dto, file);

                var response = await http.PostAsync("api/midh-schemes/save-midh-scheme-data", content);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save midh scheme: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateMIDHSchemeAsync(AddMIDHSchemeDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = BuildMultipartContent(dto, file);

                var response = await http.PostAsync("api/midh-schemes/update-midh-scheme-data", content);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update midh scheme: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMIDHSchemeActiveFlagAsync(int? schemeId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/midh-schemes/{schemeId}/active-flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update active flag: {ex.Message}");
            }
        }

        private static MultipartFormDataContent BuildMultipartContent(AddMIDHSchemeDto dto, IBrowserFile? file)
        {
            var content = new MultipartFormDataContent();

            if (dto.SchemeId is not null)
            {
                content.Add(new StringContent(dto.SchemeId.Value.ToString()), nameof(dto.SchemeId));
            }

            if (dto.MIDHSchemeId is not null)
            {
                content.Add(new StringContent(dto.MIDHSchemeId.Value.ToString()), nameof(dto.MIDHSchemeId));
            }

            content.Add(new StringContent(dto.SchemeTypeId?.ToString() ?? string.Empty), nameof(dto.SchemeTypeId));
            content.Add(new StringContent(dto.MIDHSchemeCode ?? string.Empty), nameof(dto.MIDHSchemeCode));
            content.Add(new StringContent(dto.MIDHSchemeNameHi ?? string.Empty), nameof(dto.MIDHSchemeNameHi));
            content.Add(new StringContent(dto.MIDHSchemeNameEn ?? string.Empty), nameof(dto.MIDHSchemeNameEn));
            content.Add(new StringContent(dto.MIDHSchemeDescriptionHi ?? string.Empty), nameof(dto.MIDHSchemeDescriptionHi));
            content.Add(new StringContent(dto.MIDHSchemeDescriptionEn ?? string.Empty), nameof(dto.MIDHSchemeDescriptionEn));

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