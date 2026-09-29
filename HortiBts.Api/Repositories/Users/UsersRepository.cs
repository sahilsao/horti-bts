using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Users;

namespace HortiBts.Api.Repositories.Users
{
    public interface IUsersRepository
    {
        // district users password reset
        Task<Result<List<DistrictUsersDto>>> GetDistrictsUsersAsync(int departmentCode, int userType);

        Task<Result<int>> DistrictResetPasswordAsync(
            int districtCode, int departmentCode, int userType,
            string passwordHash, int passwordFlag, string? ipAddress);

        // shdo user password reset

        Task<Result<int>> ShdoResetPasswordAsync(
        int officerCode, int userType,
        string passwordHash, int passwordFlag, string? ipAddress);

        // rheo user password reset

        Task<Result<int>> RheoResetPasswordAsync(
        int officerCode, int userType,
        string passwordHash, int passwordFlag, string? ipAddress);
    }

    public class UsersRepository(IDbConnectionFactory dbFactory) : IUsersRepository
    {
        public async Task<Result<List<DistrictUsersDto>>> GetDistrictsUsersAsync(int departmentCode, int userType)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT
                    tl.district_code,
                    tl.district_name,
                    rd.DistrictNameHindi,
                    tl.department_code,
                    tl.department_name_eng,
                    tl.department_name,
                    tl.usertype
                FROM
                    tbl_login tl
                INNER JOIN rev_district rd ON tl.district_code = rd.DistrictCensus	
                WHERE
                    tl.department_code = @DepartmentCode
                    AND tl.usertype = @UserType
                ORDER BY
                    tl.district_name
                """;
                var result = await connection.QueryAsync<DistrictUsersDto>(sql, new { DepartmentCode = departmentCode, UserType = userType });
                return Result<List<DistrictUsersDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<DistrictUsersDto>>.Failure($"Failed to fetch districts users: {ex.Message}");
            }
        }

        public async Task<Result<int>> DistrictResetPasswordAsync(
        int districtCode, int departmentCode, int userType,
        string passwordHash, int passwordFlag, string? ipAddress)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                UPDATE tbl_login
                SET PASSWORD      = @PasswordHash,
                    password_flag = @PasswordFlag,
                    ipaddress     = @IpAddress
                WHERE district_code   = @DistrictCode
                AND department_code = @DepartmentCode
                AND usertype        = @UserType
                """;

                var rows = await connection.ExecuteAsync(sql, new
                {
                    PasswordHash = passwordHash,
                    PasswordFlag = passwordFlag,
                    IpAddress = ipAddress,
                    DistrictCode = districtCode,
                    DepartmentCode = departmentCode,
                    UserType = userType
                });

                return Result<int>.Success(rows);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to reset password: {ex.Message}");
            }
        }

        public async Task<Result<int>> ShdoResetPasswordAsync(
        int officerCode, int userType,
        string passwordHash, int passwordFlag, string? ipAddress)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                UPDATE mas_raeo
                SET PASSWORD      = @PasswordHash,
                    password_flag = @PasswordFlag,
                    ipaddress     = @IpAddress
                WHERE officer_code   = @OfficerCode
                AND usertype        = @UserType
                """;

                var rows = await connection.ExecuteAsync(sql, new
                {
                    PasswordHash = passwordHash,
                    PasswordFlag = passwordFlag,
                    IpAddress = ipAddress,
                    OfficerCode = officerCode,
                    UserType = userType
                });

                return Result<int>.Success(rows);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to reset password: {ex.Message}");
            }
        }

        public async Task<Result<int>> RheoResetPasswordAsync(
        int officerCode, int userType,
        string passwordHash, int passwordFlag, string? ipAddress)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                UPDATE mas_raeo
                SET PASSWORD      = @PasswordHash,
                    password_flag = @PasswordFlag,
                    ipaddress     = @IpAddress
                WHERE officer_code   = @OfficerCode
                AND usertype        = @UserType
                """;

                var rows = await connection.ExecuteAsync(sql, new
                {
                    PasswordHash = passwordHash,
                    PasswordFlag = passwordFlag,
                    IpAddress = ipAddress,
                    OfficerCode = officerCode,
                    UserType = userType
                });

                return Result<int>.Success(rows);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to reset password: {ex.Message}");
            }
        }
    }
}
