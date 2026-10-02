using HortiBts.Api.Helpers;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Auth;
using HortiBts.Shared.Enums.Auth;

namespace HortiBts.Api.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<Result<LoginResponseDto>> LoginAsync(LoginType loginType, string loginId, string password);
        Task<Result<LoginResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<Result<PasswordUpdateResult>> ChangeAdminPasswordAsync(string currentPassword, string newPassword);
        Task<Result<PasswordUpdateResult>> ChangeDistrictPasswordAsync(string currentPassword, string newPassword);
        Task<Result<PasswordUpdateResult>> ResetAdminPasswordAsync(string userId, string newPassword);
        Task LogoutAsync(string? refreshToken = null);
    }

    internal class AuthRepository(
        ILoginRepository loginRepository,
        ILoginHistoryRepository loginHistoryRepository,
        IPasswordRepository passwordRepository,
        IJwtTokenRepository jwtTokenRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IHttpContextAccessor httpContextAccessor) : IAuthRepository
    {

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginType loginType, string loginId, string password)
        {
            var user = await loginRepository.VerifyAsync(loginType, loginId, password);

            if (user is null)
                return Result<LoginResponseDto>.Failure("Invalid User ID or Password.");

            var ip = IpAddressHelper.GetClientIp(httpContextAccessor);

            var (accessToken, accessExpires, loginHistoryId) = jwtTokenRepository.GenerateToken(user, ip);

            var (refreshToken, refreshExpires) =
                jwtTokenRepository.GenerateRefreshToken();

            await refreshTokenRepository.ReplaceAsync(
                user.UserId,
                refreshToken,
                refreshExpires,
                ip);

            await loginHistoryRepository.AddAsync(
                int.Parse(user.UserId),
                user.UserType.ToString(),
                loginHistoryId,
                accessToken,
                ip,
                httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString());

            return Result<LoginResponseDto>.Success(
                     new LoginResponseDto(
                         accessToken,
                         accessExpires,
                         refreshToken,
                         refreshExpires,

                         user.UserId,
                         GetRoleName(user.UserType),

                         user.UserType,

                         user.UsernameEn,
                         user.UsernameHi,
                         user.DistrictCode,
                         user.SubDistrictCode,

                         user.PasswordFlag));
        }

        public async Task<Result<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var existing = await refreshTokenRepository.ValidateAsync(refreshToken);
            if (existing is null)
                return Result<LoginResponseDto>.Failure("Invalid or expired refresh token.");

            var user = await loginRepository.GetByUserIdAsync(existing.UserId);

            if (user is null)
            {
                await refreshTokenRepository.RevokeAsync(existing);
                return Result<LoginResponseDto>.Failure("User not found.");
            }

            var ip = IpAddressHelper.GetClientIp(httpContextAccessor);

            var (accessToken, accessExpires, loginHistoryId) = jwtTokenRepository.GenerateToken(user, ip);

            var (newRefreshToken, refreshExpires) = jwtTokenRepository.GenerateRefreshToken();

            await refreshTokenRepository.ReplaceAsync(
                    user.UserId,
                    newRefreshToken,
                    refreshExpires,
                    ip);

            await loginHistoryRepository.AddAsync(
                    int.Parse(user.UserId),
                    user.UserType.ToString(),
                    loginHistoryId,
                    accessToken,
                    ip,
                    httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString());

            return Result<LoginResponseDto>.Success(
                  new LoginResponseDto(
                      accessToken,
                      accessExpires,
                      newRefreshToken,
                      refreshExpires,

                    user.UserId,
                  GetRoleName(user.UserType),

                  user.UserType,

                  user.UsernameEn,
                  user.UsernameHi,

                  user.DistrictCode,
                  user.SubDistrictCode,

                  user.PasswordFlag));
        }

        public async Task<Result<PasswordUpdateResult>> ChangeAdminPasswordAsync(string currentPassword, string newPassword)
        {
            var userId = httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Result<PasswordUpdateResult>.Failure("Not authenticated.");

            var result = await passwordRepository.UpdateAdminPasswordAsync(userId, currentPassword, newPassword);
            return Result<PasswordUpdateResult>.Success(result);
        }

        public async Task<Result<PasswordUpdateResult>> ChangeDistrictPasswordAsync(string currentPassword, string newPassword)
        {
            var userId = httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Result<PasswordUpdateResult>.Failure("Not authenticated.");

            var result = await passwordRepository.UpdateDistrictPasswordAsync(userId, currentPassword, newPassword);
            return Result<PasswordUpdateResult>.Success(result);
        }

        public async Task<Result<PasswordUpdateResult>> ResetAdminPasswordAsync(string userId, string newPassword)
        {
            var result = await passwordRepository.ResetAdminPasswordAsync(userId, newPassword);
            return Result<PasswordUpdateResult>.Success(result);
        }

        public async Task LogoutAsync(string? refreshToken = null)
        {
            if (string.IsNullOrEmpty(refreshToken)) return;

            var existing = await refreshTokenRepository.ValidateAsync(refreshToken);
            if (existing is not null)
            {
                await refreshTokenRepository.RevokeAsync(existing);
            }

            var claim = httpContextAccessor.HttpContext?.User.FindFirst("lh_id")?.Value;

            if (long.TryParse(claim, out var loginHistoryId))
            {
                await loginHistoryRepository.LogoutAsync(loginHistoryId);
            }
        }

        private static string GetRoleName(int userType)
        {
            return userType switch
            {
                13 => "Admin",
                14 => "District",
                1 => "District",
                12 => "RHEO",
                3 => "RHEO",
                _ => "User"
            };
        }
    }
}