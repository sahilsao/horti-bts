using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Auth;
using HortiBts.Shared.Enums.Auth;

namespace HortiBts.Api.Repository.Auth
{
    public interface IAuthRepository
    {
        Task<Result<LoginResponseDto>> LoginAsync(string username, string password);
        Task<Result<LoginResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<Result<PasswordUpdateResult>> ChangePasswordAsync(string currentPassword, string newPassword);
        Task<Result<PasswordUpdateResult>> AdminResetPasswordAsync(string userId, string newPassword);
        Task<Result<List<AccountDto>>> GetDistrictAccountsAsync();
        Task LogoutAsync(string? refreshToken = null);
    }

    internal class AuthRepository(
        ILoginRepository loginRepository,
        IPasswordRepository passwordRepository,
        IJwtTokenRepository jwtTokenRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IHttpContextAccessor httpContextAccessor) : IAuthRepository
    {
        private string? GetClientIp() =>
            httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"]
                .ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim()
            ?? httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        public async Task<Result<LoginResponseDto>> LoginAsync(string username, string password)
        {
            var user = await loginRepository.VerifyAsync(username, password);
            if (user is null)
                return Result<LoginResponseDto>.Failure("Invalid User ID or Password.");

            var ip = GetClientIp();

            var (accessToken, accessExpires) = jwtTokenRepository.GenerateToken(user, ip);
            var (refreshToken, refreshExpires) = jwtTokenRepository.GenerateRefreshToken();

            await refreshTokenRepository.StoreAsync(user.UserId, refreshToken, refreshExpires, ip);

            return Result<LoginResponseDto>.Success(new LoginResponseDto(
                accessToken, accessExpires,
                refreshToken, refreshExpires,
                user.UserId,
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.Role.ToLower()),
                user.UsernameEn, user.UsernameHi));
        }

        public async Task<Result<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var existing = await refreshTokenRepository.ValidateAsync(refreshToken);
            if (existing is null)
                return Result<LoginResponseDto>.Failure("Invalid or expired refresh token.");

            var user = await loginRepository.GetByIdAsync(existing.UserId);
            if (user is null)
                return Result<LoginResponseDto>.Failure("User not found.");

            var ip = GetClientIp();

            var (accessToken, accessExpires) = jwtTokenRepository.GenerateToken(user, ip);
            var (newRefreshToken, refreshExpires) = jwtTokenRepository.GenerateRefreshToken();

            var newRecord = await refreshTokenRepository.StoreAsync(user.UserId, newRefreshToken, refreshExpires, ip);
            await refreshTokenRepository.RevokeAsync(existing, replacedByHash: newRecord.TokenHash);

            return Result<LoginResponseDto>.Success(new LoginResponseDto(
                accessToken, accessExpires,
                newRefreshToken, refreshExpires,
                user.UserId,
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.Role.ToLower()),
                user.UsernameEn, user.UsernameHi));
        }

        public async Task<Result<PasswordUpdateResult>> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var userId = httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Result<PasswordUpdateResult>.Failure("Not authenticated.");

            var result = await passwordRepository.UpdatePasswordAsync(userId, currentPassword, newPassword);
            return Result<PasswordUpdateResult>.Success(result);
        }

        public async Task<Result<PasswordUpdateResult>> AdminResetPasswordAsync(string userId, string newPassword)
        {
            var result = await passwordRepository.AdminResetPasswordAsync(userId, newPassword);
            return Result<PasswordUpdateResult>.Success(result);
        }

        public async Task<Result<List<AccountDto>>> GetDistrictAccountsAsync()
        {
            var result = await passwordRepository.GetDistrictAccountsAsync();
            return Result<List<AccountDto>>.Success(result);
        }

        public async Task LogoutAsync(string? refreshToken = null)
        {
            if (string.IsNullOrEmpty(refreshToken)) return;

            var existing = await refreshTokenRepository.ValidateAsync(refreshToken);
            if (existing is not null)
                await refreshTokenRepository.RevokeAsync(existing);
        }
    }
}