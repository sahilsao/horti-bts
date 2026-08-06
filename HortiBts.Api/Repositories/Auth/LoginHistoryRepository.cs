using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Models.Auth;

namespace HortiBts.Api.Repositories.Auth
{
    public interface ILoginHistoryRepository
    {
        Task<bool> IsAlreadyLoggedInAsync(int userId);

        Task AddAsync(int userId, string userType, long loginHistoryId, string jwtToken, string? ipAddress, string? browserVersion);

        Task LogoutAsync(long loginHistoryId);

        Task<LoginHistoryRecord?> GetActiveLoginAsync(int userId, string userType);
    }
    internal class LoginHistoryRepository(IDbConnectionFactory dbFactory) : ILoginHistoryRepository
    {
        public async Task<bool> IsAlreadyLoggedInAsync(int userId)
        {
            const string sql = """
            SELECT COUNT(*)
            FROM login_history
            WHERE user_id=@UserId
              AND status=1
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            return await connection.ExecuteScalarAsync<int>(
                sql,
                new { UserId = userId }) > 0;
        }

        public async Task AddAsync(int userId, string userType, long loginHistoryId, string jwt, string? ip, string? browser)
        {
            const string sql = """
            INSERT INTO login_history
            (
                ip_address,
                user_id,
                user_type,
                lh_id,
                auth_token,
                status,
                browser_version
            )
            VALUES
            (
                @Ip,
                @UserId,
                @UserType,
                @LoginHistoryId,
                @Jwt,
                1,
                @Browser
            )
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            await connection.ExecuteAsync(sql, new
            {
                Ip = ip,
                UserId = userId,
                UserType = userType,
                LoginHistoryId = loginHistoryId,
                Jwt = jwt,
                Browser = browser
            });
        }
        public async Task LogoutAsync(long loginHistoryId)
        {
            const string sql = """
            UPDATE login_history
            SET
                status=0,
                time_out=NOW()
            WHERE lh_id=@LoginHistoryId
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            await connection.ExecuteAsync(sql,
                new { LoginHistoryId = loginHistoryId });
        }

        public async Task<LoginHistoryRecord?> GetActiveLoginAsync(int userId, string userType)
        {
            const string sql = """
            SELECT
                id                  AS Id,
                user_id             AS UserId,
                user_type           AS UserType,
                lh_id               AS LoginHistoryId,
                auth_token          AS AuthToken,
                time_in             AS TimeIn,
                time_out            AS TimeOut,
                ip_address          AS IpAddress,
                status              AS Status,
                browser_version     AS BrowserVersion
            FROM login_history
            WHERE user_id = @UserId
              AND user_type = @UserType
              AND status = 1
            ORDER BY id DESC
            LIMIT 1
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            return await connection.QuerySingleOrDefaultAsync<LoginHistoryRecord>(
                sql,
                new
                {
                    UserId = userId,
                    UserType = userType
                });
        }
    }
}
