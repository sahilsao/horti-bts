using HortiBts.Client.Pages.Farmers.FarmerDetails.Models;
using HortiBts.Shared.Common;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Girdawari
{
    public class GirdawariApiService(HttpClient httpClient, ILogger<GirdawariApiService> logger)
    {
        public async Task<Result<List<GirdawariRecord>>> GetGirdawariDetailsAsync(
            IReadOnlyList<SearchParam> searchParams,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await httpClient.PostAsJsonAsync(
                    "api/landrecord/get-girdawari-details", searchParams, cancellationToken);

                return await ApiResultHelper.ReadResultAsync<List<GirdawariRecord>>(response, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "getgirdawaridetails call failed for {Count} search params", searchParams.Count);
                return Result<List<GirdawariRecord>>.Failure("Could not reach the land record service.");
            }
        }
    }
}
