using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HortiBts.Client.Services.Auth
{
    public class TokenAuthenticationStateProvider(IJSRuntime js) : AuthenticationStateProvider
    {
        private const string AccessTokenKey = "Hortibts_token";
        private const string RefreshTokenKey = "Hortibts_refresh_token";

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey);
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrWhiteSpace(token) && !IsExpired(token))
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            else if (!string.IsNullOrWhiteSpace(token))
                await js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task MarkUserAsAuthenticated(string accessToken, string refreshToken)
        {
            await js.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
            await js.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task UpdateTokensAsync(string accessToken, string refreshToken)
        {
            await js.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
            await js.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
            await js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
            var identity = new ClaimsIdentity();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task<string?> GetTokenAsync()
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey);
            return string.IsNullOrWhiteSpace(token) || IsExpired(token) ? null : token;
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", RefreshTokenKey);
            return string.IsNullOrWhiteSpace(token) ? null : token;
        }

        private static bool IsExpired(string jwt)
        {
            var claims = ParseClaimsFromJwt(jwt);
            var exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (exp is null) return true;
            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp));
            return expDate < DateTimeOffset.UtcNow;
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var json = Encoding.UTF8.GetString(ParseBase64WithoutPadding(payload));
            var kvp = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;

            return kvp.Select(kv => new Claim(
                kv.Key switch
                {
                    "role" or "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role" => ClaimTypes.Role,
                    "unique_name" or "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" => ClaimTypes.Name,
                    _ => kv.Key
                },
                kv.Value.ToString() ?? ""));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            base64 = base64.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}