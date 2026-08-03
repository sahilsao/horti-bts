using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Models.Auth;
using System.Security.Cryptography;
using System.Text;

namespace HortiBts.Api.Repository.Auth
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenRecord> StoreAsync(string userId, string rawToken, DateTime expires, string? ip);
        Task<RefreshTokenRecord?> ValidateAsync(string rawToken);
        Task RevokeAsync(RefreshTokenRecord token, string? replacedByHash = null);
        Task RevokeAllForUserAsync(string userId);
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
            INSERT INTO mas_refresh_tokens (userid, token_hash, expires_at, created_at, created_by_ip)
            VALUES (@UserId, @TokenHash, @ExpiresAt, UTC_TIMESTAMP(), @CreatedByIp);
            SELECT LAST_INSERT_ID();
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            var hash = Hash(rawToken);

            var id = await connection.ExecuteScalarAsync<long>(sql, new
            {
                UserId = userId,
                TokenHash = hash,
                ExpiresAt = expires,
                CreatedByIp = ip
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

        public async Task<RefreshTokenRecord?> ValidateAsync(string rawToken)
        {
            const string sql = """
            SELECT
                id AS Id, userid AS UserId, token_hash AS TokenHash,
                expires_at AS ExpiresAt, created_at AS CreatedAt,
                revoked_at AS RevokedAt, replaced_by_token_hash AS ReplacedByTokenHash,
                created_by_ip AS CreatedByIp
            FROM mas_refresh_tokens
            WHERE token_hash = @TokenHash
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            var hash = Hash(rawToken);
            var token = await connection.QuerySingleOrDefaultAsync<RefreshTokenRecord>(sql, new { TokenHash = hash });

            if (token is null) return null;

            if (token.RevokedAt is not null)
            {
                await RevokeAllForUserAsync(token.UserId);
                return null;
            }

            return token.IsActive ? token : null;
        }

        public async Task RevokeAsync(RefreshTokenRecord token, string? replacedByHash = null)
        {
            const string sql = """
            UPDATE mas_refresh_tokens
            SET revoked_at = UTC_TIMESTAMP(), replaced_by_token_hash = @ReplacedByHash
            WHERE id = @Id
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            await connection.ExecuteAsync(sql, new { token.Id, ReplacedByHash = replacedByHash });
        }

        public async Task RevokeAllForUserAsync(string userId)
        {
            const string sql = """
            UPDATE mas_refresh_tokens
            SET revoked_at = UTC_TIMESTAMP()
            WHERE userid = @UserId AND revoked_at IS NULL
            """;

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            await connection.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}