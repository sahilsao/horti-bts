using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Models.Auth;
using System.Security.Cryptography;
using System.Text;

namespace HortiBts.Api.Repositories.Auth
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenRecord> StoreAsync(string userId, string rawToken, DateTime expires, string? ip);
        Task<RefreshTokenRecord> ReplaceAsync(string userId, string rawToken, DateTime expires, string? ip);
        Task<RefreshTokenRecord?> ValidateAsync(string rawToken);
        Task RevokeAsync(RefreshTokenRecord token, string? replacedByHash = null);
        Task RevokeAllForUserAsync(string userId);
        Task<bool> HasActiveTokenAsync(string userId);
    }
    internal class RefreshTokenRepository(IDbConnectionFactory dbFactory) : IRefreshTokenRepository
    {
        private static string Hash(string raw)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(bytes);
        }

        public async Task<RefreshTokenRecord> StoreAsync(string userId, string rawToken, DateTime expires, string? ip)
        {
            const string sql = """
                                INSERT INTO refresh_tokens
                                (
                                    token,
                                    client_id,
                                    expires_at,
                                    created_at,
                                    created_by,
                                    ip_address
                                )
                                VALUES
                                (
                                    @Token,
                                    @ClientId,
                                    @ExpiresAt,
                                    UTC_TIMESTAMP(),
                                    @CreatedBy,
                                    @IpAddress
                                );

                                SELECT LAST_INSERT_ID();
                                """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            var hash = Hash(rawToken);

            var clientId = long.Parse(userId);

            var id = await connection.ExecuteScalarAsync<long>(sql, new
            {
                Token = hash,
                ClientId = clientId,
                ExpiresAt = expires,
                CreatedBy = clientId,
                IpAddress = ip
            });

            return new RefreshTokenRecord
            {
                Id = id,
                UserId = userId,
                TokenHash = hash,
                ExpiresAt = expires,
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ip
            };
        }

        public async Task<RefreshTokenRecord> ReplaceAsync(string userId, string rawToken, DateTime expires, string? ip)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                var clientId = long.Parse(userId);
                var hash = Hash(rawToken);

                await connection.ExecuteAsync(
                    """
                    DELETE FROM refresh_tokens
                    WHERE client_id = @ClientId;
                    """,
                    new { ClientId = clientId },
                    transaction);

                var id = await connection.ExecuteScalarAsync<long>(
                    """
                    INSERT INTO refresh_tokens
                    (
                        token,
                        client_id,
                        expires_at,
                        created_at,
                        created_by,
                        ip_address
                    )
                    VALUES
                    (
                        @Token,
                        @ClientId,
                        @ExpiresAt,
                        UTC_TIMESTAMP(),
                        @CreatedBy,
                        @IpAddress
                    );

                    SELECT LAST_INSERT_ID();
                    """,
                    new
                    {
                        Token = hash,
                        ClientId = clientId,
                        ExpiresAt = expires,
                        CreatedBy = clientId,
                        IpAddress = ip
                    },
                    transaction);

                await transaction.CommitAsync();

                return new RefreshTokenRecord
                {
                    Id = id,
                    UserId = userId,
                    TokenHash = hash,
                    ExpiresAt = expires,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByIp = ip
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<RefreshTokenRecord?> ValidateAsync(string rawToken)
        {
            const string sql = """
            SELECT
                id          AS Id,
                client_id   AS UserId,
                token       AS TokenHash,
                expires_at  AS ExpiresAt,
                created_at  AS CreatedAt,
                ip_address  AS CreatedByIp
            FROM refresh_tokens
            WHERE token = @Token
            LIMIT 1
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            var hash = Hash(rawToken);

            var token = await connection.QuerySingleOrDefaultAsync<RefreshTokenRecord>(
                sql,
                new { Token = hash });

            if (token is null)
                return null;

            return token.IsActive ? token : null;
        }

        public async Task RevokeAsync(RefreshTokenRecord token, string? replacedByHash = null)
        {
            const string sql = """
            DELETE FROM refresh_tokens
            WHERE id = @Id
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            await connection.ExecuteAsync(sql, new { token.Id });
        }

        public async Task RevokeAllForUserAsync(string userId)
        {
            const string sql = """
            DELETE FROM refresh_tokens
            WHERE client_id = @UserId
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            await connection.ExecuteAsync(sql, new
            {
                UserId = long.Parse(userId)
            });
        }

        public async Task<bool> HasActiveTokenAsync(string userId)
        {
            const string sql = """
            SELECT COUNT(*)
            FROM refresh_tokens
            WHERE client_id = @UserId
              AND expires_at > UTC_TIMESTAMP()
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            return await connection.ExecuteScalarAsync<int>(
                sql,
                new { UserId = long.Parse(userId) }) > 0;
        }
    }
}