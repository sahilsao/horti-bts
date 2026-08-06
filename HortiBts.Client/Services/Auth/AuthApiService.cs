using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Auth;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using HortiBts.Shared.Enums.Auth;

namespace HortiBts.Client.Services.Auth
{
    public class AuthApiService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        private readonly TokenAuthenticationStateProvider _authStateProvider = (TokenAuthenticationStateProvider)authStateProvider;

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginType loginType, string userId, string password)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/auth/login", new LoginRequestDto
                {
                    LoginType = loginType,
                    UserId = userId,
                    Password = password
                });
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Result<LoginResponseDto>.Failure(string.IsNullOrWhiteSpace(error) ? "Invalid User ID or Password." : error);
                }

                var data = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (data is null) return Result<LoginResponseDto>.Failure("Empty response from server.");

                await _authStateProvider.MarkUserAsAuthenticated(data.AccessToken, data.RefreshToken);
                return Result<LoginResponseDto>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<LoginResponseDto>.Failure($"Login failed: {ex.Message}");
            }
        }

        public async Task<Result<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/auth/refresh", new RefreshTokenRequestDto
                {
                    RefreshToken = refreshToken
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Result<LoginResponseDto>.Failure(string.IsNullOrWhiteSpace(error) ? "Session expired." : error);
                }

                var data = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (data is null) return Result<LoginResponseDto>.Failure("Empty response from server.");

                await _authStateProvider.UpdateTokensAsync(data.AccessToken, data.RefreshToken);
                return Result<LoginResponseDto>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<LoginResponseDto>.Failure($"Refresh failed: {ex.Message}");
            }
        }

        public async Task<Result<PasswordUpdateResult>> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/auth/change-password", new ChangePasswordDto(currentPassword, newPassword));
                if (!response.IsSuccessStatusCode)
                    return Result<PasswordUpdateResult>.Failure(await response.Content.ReadAsStringAsync());

                var result = await response.Content.ReadFromJsonAsync<PasswordUpdateResult>();
                return Result<PasswordUpdateResult>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<PasswordUpdateResult>.Failure($"Failed to update password: {ex.Message}");
            }
        }

        public async Task<Result<PasswordUpdateResult>> AdminResetPasswordAsync(string userId, string newPassword)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/auth/admin-reset-password", new AdminResetPasswordDto(userId, newPassword));
                if (!response.IsSuccessStatusCode)
                    return Result<PasswordUpdateResult>.Failure(await response.Content.ReadAsStringAsync());

                var result = await response.Content.ReadFromJsonAsync<PasswordUpdateResult>();
                return Result<PasswordUpdateResult>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<PasswordUpdateResult>.Failure($"Failed to reset password: {ex.Message}");
            }
        }

        public async Task LogoutAsync(string? refreshToken = null)
        {
            var token = refreshToken ?? await _authStateProvider.GetRefreshTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    await http.PostAsJsonAsync(
                    "api/auth/logout",
                    new RefreshTokenRequestDto
                    {
                        RefreshToken = token
                    });
                }
                catch { }
            }

            await _authStateProvider.MarkUserAsLoggedOut();
        }
    }
}