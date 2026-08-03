using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Models.Auth;

namespace HortiBts.Api.Repository.Auth
{
    internal interface ILoginRepository
    {
        Task<LoginRecord?> VerifyAsync(string userId, string password);
        Task<LoginRecord?> GetByIdAsync(string userId);
    }

    internal class LoginRepository(IDbConnectionFactory dbFactory) : ILoginRepository
    {
        private const string SelectUserSql = """
            SELECT
                maa.userid AS UserId,
                maa.password AS Password,
                maa.role AS Role,
                CASE WHEN maa.userid = 'admin' THEN 'Administrator' ELSE md.district_name_en END AS UsernameEn,
                CASE WHEN maa.userid = 'admin' THEN 'एडमिन' ELSE md.district_name_hi END AS UsernameHi
            FROM mas_attendance_accounts maa
            LEFT JOIN mas_district_office md
                ON (maa.userid REGEXP '^[0-9]+$' AND CAST(maa.userid AS UNSIGNED) = md.district_id)
            WHERE maa.userid = @userid
            """;

        public async Task<LoginRecord?> VerifyAsync(string userId, string password)
        {
            var record = await GetByIdAsync(userId);
            if (record is null) return null;
            return BCrypt.Net.BCrypt.Verify(password, record.Password) ? record : null;
        }

        public async Task<LoginRecord?> GetByIdAsync(string userId)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            return await connection.QueryFirstOrDefaultAsync<LoginRecord>(SelectUserSql, new { userid = userId });
        }
    }
}