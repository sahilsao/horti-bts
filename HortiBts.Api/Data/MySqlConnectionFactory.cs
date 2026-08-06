using MySqlConnector;
using System.Data;

namespace HortiBts.Api.Data;

public enum HortiDb
{
    Bts,       // maps to "HortiDbConn"
}

public interface IDbConnectionFactory
{
    MySqlConnection CreateConnection(HortiDb db = HortiDb.Bts);
}

public class MySqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private static readonly Dictionary<HortiDb, string> ConnectionNames = new()
    {
        [HortiDb.Bts] = "HortiDbConn",
    };

    public MySqlConnection CreateConnection(HortiDb db = HortiDb.Bts)
    {
        var name = ConnectionNames[db];
        var connStr = configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Missing ConnectionStrings:{name} in appsettings.json");
        return new MySqlConnection(connStr);
    }
}
