using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Notices;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Notices
{
    public class NoticesApiService(HttpClient http)
    {

        public async Task<Result<List<NoticesDto>>> GetNoticesListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/notices");
                return await ApiResultHelper.ReadResultAsync<List<NoticesDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NoticesDto>>.Failure($"Failed to fetch notices: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveNoticeAsync(AddNoticesDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = BuildMultipartContent(dto, file);

                var response = await http.PostAsync("api/notices/save-notice-data", content);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save notice: {ex.Message}");
            }
        }
        public async Task<Result<int>> UpdateNoticeAsync(AddNoticesDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = BuildMultipartContent(dto, file);

                var response = await http.PostAsync("api/notices/update-notice-data", content);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to update notice: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateNoticeActiveFlagAsync(int noticeId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/notices/{noticeId}/active-flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update active flag: {ex.Message}");
            }
        }

        private static MultipartFormDataContent BuildMultipartContent(AddNoticesDto dto, IBrowserFile? file)
        {
            var content = new MultipartFormDataContent();

            if (dto.NoticeId > 0)

                content.Add(new StringContent(dto.NoticeId.ToString()), nameof(dto.NoticeId));
            content.Add(new StringContent(dto.NoticeType?.ToString() ?? string.Empty), nameof(dto.NoticeType));
            content.Add(new StringContent(dto.Status ?? string.Empty), nameof(dto.Status));
            content.Add(new StringContent(dto.Description ?? string.Empty), nameof(dto.Description));
            content.Add(new StringContent(dto.StartDate.ToString() ?? string.Empty), nameof(dto.StartDate));
            content.Add(new StringContent(dto.EndDate.ToString() ?? string.Empty), nameof(dto.EndDate));
            content.Add(new StringContent(dto.Priority!.ToString() ?? string.Empty), nameof(dto.Priority));
            content.Add(new StringContent(dto.Subject.ToString() ?? string.Empty), nameof(dto.Subject));

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