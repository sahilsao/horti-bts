using Dapper;
using HortiBts.Api.Data;

namespace HortiBts.Api.Repositories.Auth
{
    public class RefreshTokenCleanupRepository(IServiceProvider services, ILogger<RefreshTokenCleanupRepository> logger) : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
                    using var connection = db.CreateConnection(HortiDb.Bts);

                    const string sql = """
                    DELETE FROM mas_refresh_tokens
                    WHERE expires_at < UTC_TIMESTAMP()
                       OR (revoked_at IS NOT NULL AND revoked_at < DATE_SUB(UTC_TIMESTAMP(), INTERVAL 30 DAY))
                    """;

                    var deleted = await connection.ExecuteAsync(sql);
                    if (deleted > 0)
                    {
                        logger.LogInformation("Refresh token cleanup removed {Count} rows.", deleted);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Refresh token cleanup failed.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}