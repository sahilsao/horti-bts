using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Dtos.Auth;
using HortiBts.Shared.Enums.Auth;

namespace HortiBts.Api.Repositories.Auth
{
    internal interface IPasswordRepository
    {
        Task<PasswordUpdateResult> UpdatePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<PasswordUpdateResult> AdminResetPasswordAsync(string userId, string newPassword);
    }

    public class PasswordRepository(IDbConnectionFactory dbFactory) : IPasswordRepository
    {
        public async Task<PasswordUpdateResult> UpdatePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            const string fetchSql = "";
            const string updateSql = "";

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            var storedHash = await connection.QuerySingleOrDefaultAsync<string>(fetchSql, new { UserId = userId });
            if (storedHash is null) return PasswordUpdateResult.UserNotFound;

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, storedHash))
                return PasswordUpdateResult.WrongCurrentPassword;

            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await connection.ExecuteAsync(updateSql, new { NewPassword = newHash, UserId = userId });
            return PasswordUpdateResult.Success;
        }

        public async Task<PasswordUpdateResult> AdminResetPasswordAsync(string userId, string newPassword)
        {
            const string existsSql = "";
            const string updateSql = "";

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            var exists = await connection.QuerySingleOrDefaultAsync<string>(existsSql, new { UserId = userId });
            if (exists is null) return PasswordUpdateResult.UserNotFound;

            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await connection.ExecuteAsync(updateSql, new { NewPassword = newHash, UserId = userId });
            return PasswordUpdateResult.Success;
        }
    }
}

