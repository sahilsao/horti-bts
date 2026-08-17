using HortiBts.Client.Pages.Farmers.FarmerDetails.Models;
using HortiBts.Shared.Common;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Girdawari
{
    public class GirdawariApiService(HttpClient httpClient, ILogger<GirdawariApiService> logger)
    {
        public async Task<Result<List<List<GirdawariRecord>>>> GetGirdawariDetailsAsync(
            IReadOnlyList<SearchParam> searchParams,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await httpClient.PostAsJsonAsync(
                    "api/landrecord/get-girdawari-details", searchParams, cancellationToken);

                // The API returns a Result<T> body even on 400/502 — read it either way.
                var result = await response.Content.ReadFromJsonAsync<Result<List<List<GirdawariRecord>>>>(
                    cancellationToken: cancellationToken);

                return result ?? Result<List<List<GirdawariRecord>>>.Failure("Empty response from server.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "getgirdawaridetails call failed for {Count} search params", searchParams.Count);
                return Result<List<List<GirdawariRecord>>>.Failure("Could not reach the land record service.");
            }
        }
    }
}
