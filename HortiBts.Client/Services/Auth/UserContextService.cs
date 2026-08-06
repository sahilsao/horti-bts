using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace HortiBts.Client.Services.Auth
{
    public interface IUserContextService
    {
        Task<CurrentUserInfo> GetCurrentUserAsync();
    }

    public record CurrentUserInfo(
        string UserId,
        string UsernameEn,
        string UsernameHi,
        int UserType);

    public class UserContextService(AuthenticationStateProvider authStateProvider) : IUserContextService
    {
        public async Task<CurrentUserInfo> GetCurrentUserAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var userId = user.Identity?.Name ?? "";
            var usernameEn = user.FindFirst("username_en")?.Value ?? "";
            var usernameHi = user.FindFirst("username_hi")?.Value ?? "";

            var userTypeClaim =
                user.FindFirst("UserType")?.Value ??
                user.FindFirst(ClaimTypes.Role)?.Value ??
                "0";

            int.TryParse(userTypeClaim, out var userType);

            return new CurrentUserInfo(
                userId,
                usernameEn,
                usernameHi,
                userType);
        }
    }
}