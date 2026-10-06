using HortiBts.Shared.Common;
using HortiBts.Shared.Models.Soil;

namespace HortiBts.Client.Services.Soil
{
    public class SoilDetailsApiService(HttpClient http)
    {
        public async Task<Result<List<SoilDetailsDto>>> GetSoilDetailsAsync(
            string villageCode,
            string khasraNo,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await http.GetAsync(
                    $"api/soil?villageCode={Uri.EscapeDataString(villageCode)}&khasraNo={Uri.EscapeDataString(khasraNo)}",
                    cancellationToken);

                return await ApiResultHelper.ReadResultAsync<List<SoilDetailsDto>>(response);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Result<List<SoilDetailsDto>>.Failure($"Failed to fetch soil details: {ex.Message}");
            }
        }
    }
}
