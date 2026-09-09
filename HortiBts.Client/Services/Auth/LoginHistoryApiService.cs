using HortiBts.Api.Models.Auth;
using HortiBts.Shared.Common;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Auth
{
    public class LoginHistoryApiService(HttpClient http)
    {
        public async Task<Result<LoginHistoryRecord>> GetActiveLoginAsync(
            int userId,
            string userType)
        {
            try
            {
                var response = await http.GetAsync($"api/login-history/active?userId={userId}&userType={userType}");
                return await ApiResultHelper.ReadResultAsync<LoginHistoryRecord>(response);
            }
            catch (Exception ex)
            {
                return Result<LoginHistoryRecord>.Failure(
                    $"Failed to fetch login history: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsAlreadyLoggedInAsync(int userId)
        {
            try
            {
                var response = await http.GetAsync($"api/login-history/is-logged-in?userId={userId}");
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    $"Failed to check login status: {ex.Message}");
            }
        }

        public async Task<Result> LogoutAsync(long loginHistoryId)
        {
            try
            {
                var response = await http.PostAsync(
                    $"api/login-history/logout?loginHistoryId={loginHistoryId}",
                    null);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(await response.Content.ReadAsStringAsync());

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(
                    $"Failed to logout: {ex.Message}");
            }
        }
    }
}