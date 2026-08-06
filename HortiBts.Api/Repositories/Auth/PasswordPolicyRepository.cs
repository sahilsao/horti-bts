using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Models.Auth;
using HortiBts.Shared.Enums.Auth;

namespace HortiBts.Api.Repositories.Auth
{
    internal interface IPasswordPolicyRepository
    {
        Task<PasswordPolicyResult> CheckPolicyAsync(LoginType loginType, string loginId);
    }
    internal class PasswordPolicyRepository(IDbConnectionFactory dbFactory)
      : IPasswordPolicyRepository
    {
        public async Task<PasswordPolicyResult> CheckPolicyAsync(LoginType loginType, string loginId)
        {
            var (sql, param) = BuildQuery(loginType, loginId);

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            return await connection.QueryFirstAsync<PasswordPolicyResult>(sql, param);
        }

        private static (string Sql, object Param) BuildQuery(LoginType loginType, string loginId)
        {
            return loginType switch
            {
                LoginType.Admin => (
                    """
                    SELECT
                        password_flag = 1 AS MustChangePassword,
                        DATEDIFF(NOW(),updated_at) > 89 AS PasswordExpired,
                        updated_at AS LastChanged
                    FROM tbl_login
                    WHERE district_code=100
                    AND department_code=3
                    """,
                    new { }),

                LoginType.District => (
                    """
                    SELECT
                        password_flag = 1 AS MustChangePassword,
                        DATEDIFF(NOW(),updated_at) > 89 AS PasswordExpired,
                        updated_at AS LastChanged
                    FROM tbl_login
                    WHERE district_code=@LoginId
                    AND department_code=3
                    """,
                    new { LoginId = loginId }),

                LoginType.Rheo => (
                    """
                    SELECT
                        password_flag = 1 AS MustChangePassword,
                        DATEDIFF(NOW(),updated_at) > 89 AS PasswordExpired,
                        updated_at AS LastChanged
                    FROM mas_raeo
                    WHERE officer_code=@LoginId
                    AND usertype=3
                    """,
                    new { LoginId = loginId }),

                _ => throw new NotSupportedException()
            };
        }
    }

}
