using System.Net;
using System.Net.Http.Json;
using HortiBts.Shared.Dtos.Auth;

namespace HortiBts.Client.Services.Auth
{
    public class RefreshTokenDelegatingHandler(
        TokenAuthenticationStateProvider authStateProvider,
        IHttpClientFactory httpClientFactory) : DelegatingHandler
    {
        private static readonly SemaphoreSlim _refreshLock = new(1, 1);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                var currentToken = await authStateProvider.GetTokenAsync();
                if (string.IsNullOrWhiteSpace(currentToken))
                {
                    var refreshToken = await authStateProvider.GetRefreshTokenAsync();
                    if (string.IsNullOrWhiteSpace(refreshToken))
                    {
                        await authStateProvider.MarkUserAsLoggedOut();
                        return response;
                    }

                    var plainClient = httpClientFactory.CreateClient("Api.Refresh");
                    var refreshResponse = await plainClient.PostAsJsonAsync(
                        "api/auth/refresh", new RefreshTokenRequestDto(refreshToken), cancellationToken);

                    if (!refreshResponse.IsSuccessStatusCode)
                    {
                        await authStateProvider.MarkUserAsLoggedOut();
                        return response;
                    }

                    var data = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken: cancellationToken);
                    if (data is null)
                    {
                        await authStateProvider.MarkUserAsLoggedOut();
                        return response;
                    }

                    await authStateProvider.UpdateTokensAsync(data.AccessToken, data.RefreshToken);
                }
            }
            finally
            {
                _refreshLock.Release();
            }

            var retryRequest = await CloneRequestAsync(request);
            return await base.SendAsync(retryRequest, cancellationToken);
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
        {
            var clone = new HttpRequestMessage(original.Method, original.RequestUri)
            {
                Version = original.Version
            };

            if (original.Content is not null)
            {
                var bytes = await original.Content.ReadAsByteArrayAsync();
                clone.Content = new ByteArrayContent(bytes);
                foreach (var header in original.Content.Headers)
                    clone.Content.Headers.Add(header.Key, header.Value);
            }

            foreach (var header in original.Options)
                clone.Options.TryAdd(header.Key, header.Value);

            return clone;
        }
    }
}