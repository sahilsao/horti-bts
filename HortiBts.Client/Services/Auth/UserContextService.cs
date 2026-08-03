using Microsoft.AspNetCore.Components.Authorization;

namespace HortiBts.Client.Services.Auth
{
    public interface IUserContextService
    {
        Task<CurrentUserInfo> GetCurrentUserAsync();
    }

    public record CurrentUserInfo(string UserId, string UsernameEn, string UsernameHi);

    public class UserContextService(AuthenticationStateProvider authStateProvider) : IUserContextService
    {
        public async Task<CurrentUserInfo> GetCurrentUserAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var userId = user.Identity?.Name ?? "";
            var usernameEn = user.FindFirst("username_en")?.Value ?? "";
            var usernameHi = user.FindFirst("username_hi")?.Value ?? "";

            return new CurrentUserInfo(userId, usernameEn, usernameHi);
        }
    }
}
