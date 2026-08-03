using System.Net.Http.Headers;

namespace HortiBts.Client.Services.Auth
{
    public class AuthorizationMessageHandler(TokenAuthenticationStateProvider authStateProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await authStateProvider.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
