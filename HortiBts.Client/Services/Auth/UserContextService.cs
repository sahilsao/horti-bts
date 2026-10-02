using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using HortiBts.Client.MultiLanguage;

namespace HortiBts.Client.Services.Auth
{
    public interface IUserContextService
    {
        Task<CurrentUserInfo> GetCurrentUserAsync();

        string GetUserRoleText(CurrentUserInfo currentUser);

        string GetUserNameText(CurrentUserInfo currentUser);
    }

    public record CurrentUserInfo(
        string UserId,
        string UsernameEn,
        string UsernameHi,
        int UserType);

    public class UserContextService(
        AuthenticationStateProvider authStateProvider,
        LanguageService lang) : IUserContextService
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

        public string GetUserRoleText(CurrentUserInfo currentUser)
        {
            return currentUser.UserType switch
            {
                13 => lang.Text(
                    "Director",
                    "संचालक"),

                14 => lang.Text(
                    "DDH",
                    "उप संचालक उद्यान"),

                15 => lang.Text(
                    $"SHDO - {currentUser.UserId}",
                    $"एसएचडीओ - {currentUser.UserId}"),

                12 => lang.Text(
                    $"RHEO - {currentUser.UserId}",
                    $"आरएचईओ - {currentUser.UserId}"),

                _ => string.Empty
            };
        }

        public string GetUserNameText(CurrentUserInfo currentUser)
        {
            return lang.Text(
                currentUser.UsernameEn,
                currentUser.UsernameHi);
        }
    }
}