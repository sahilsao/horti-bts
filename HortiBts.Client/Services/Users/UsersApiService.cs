using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Districts;
using HortiBts.Shared.Dtos.Users;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Users
{
    public class UsersApiService(HttpClient http)
    {
        // district user password reset
        public async Task<Result<List<DistrictUsersDto>>> GetDistrictsUsersAsync(int departmentCode, int userType)
        {
            try
            {
                var response = await http.GetAsync($"api/users/district/get-login-users?departmentCode={departmentCode}&userType={userType}");
                return await ApiResultHelper.ReadResultAsync<List<DistrictUsersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistrictUsersDto>>.Failure($"Failed to fetch districts users: {ex.Message}");
            }
        }

        public async Task<Result<int>> DistrictResetPasswordAsync(int districtCode, int departmentCode, int userType)
        {
            try
            {
                var response = await http.PostAsJsonAsync(
                $"api/users/district/reset-password?DistrictCode={districtCode}&DepartmentCode={departmentCode}&UserType={userType}",
                new { DistrictCode = districtCode, DepartmentCode = departmentCode, UserType = userType });
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to reset password: {ex.Message}");
            }
        }

        // shdo user password reset

        public async Task<Result<int>> ShdoResetPasswordAsync(int officerCode, int userType)
        {
            try
            {
                var response = await http.PostAsJsonAsync(
                $"api/users/shdo/reset-password?OfficerCode={officerCode}&UserType={userType}",
                new { OfficerCode = officerCode, UserType = userType });
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to reset password: {ex.Message}");
            }
        }
        // rheo user password reset
        public async Task<Result<int>> RheoResetPasswordAsync(int officerCode, int userType)
        {
            try
            {
                var response = await http.PostAsJsonAsync(
                $"api/users/rheo/reset-password?OfficerCode={officerCode}&UserType={userType}",
                new { OfficerCode = officerCode, UserType = userType });
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to reset password: {ex.Message}");
            }
        }
    }
}
