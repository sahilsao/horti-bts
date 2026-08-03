using System.Text.Json;
using Microsoft.JSInterop;

namespace HortiBts.Client.Services.Common
{
    public record StorageResult<T>(bool Success, T? Value);

    public class LocalStorageService
    {
        private readonly IJSRuntime _js;
        public LocalStorageService(IJSRuntime js) => _js = js;

        public async Task<StorageResult<T>> GetAsync<T>(string key)
        {
            try
            {
                var json = await _js.InvokeAsync<string?>("localStorage.getItem", key);
                if (string.IsNullOrEmpty(json))
                    return new StorageResult<T>(false, default);

                var value = JsonSerializer.Deserialize<T>(json);
                return new StorageResult<T>(true, value);
            }
            catch
            {
                return new StorageResult<T>(false, default);
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _js.InvokeVoidAsync("localStorage.setItem", key, json);
        }
    }
}