using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;

namespace HortiBts.Api.Repositories.Dashboard.Admin;

public interface IDashboardRepository
{
    Task<Result<DashboardCountDto>> GetTotRHEOCount();

    Task<Result<DashboardCountDto>> GetTotRegFarmersCount();

    Task<Result<DashboardCountDto>> GetTotRegBacklogFarmersCount(string finYear);

    Task<Result<ApplicationDashboardDto>> GetApplicationDashboard(string finYear);
}
public class DashboardRepository(IDbConnectionFactory dbFactory) : IDashboardRepository
{
    public async Task<Result<DashboardCountDto>> GetTotRHEOCount()
    {
        try
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = """
            SELECT COUNT(1)
            FROM mas_raeo
            WHERE usertype = 3
            """;

            var count = await connection.QuerySingleAsync<int>(sql);

            return Result<DashboardCountDto>.Success(
                new DashboardCountDto
                {
                    Count = count
                });
        }
        catch (Exception ex)
        {
            return Result<DashboardCountDto>.Failure(
                $"Failed to fetch Total RHEO Count: {ex.Message}");
        }
    }

    public async Task<Result<DashboardCountDto>> GetTotRegFarmersCount()
    {
        try
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = """
            SELECT COUNT(DISTINCT fd.hf_id)
            FROM farmer_detail_horti fd
            INNER JOIN mas_farmer_horti mf
                ON fd.hf_id = mf.hf_id
            INNER JOIN equipment_details_govt_schemes e
                ON e.fd_id = fd.fd_id
            INNER JOIN view_all_villages v
                ON v.village_code = e.village_code
            """;

            var count = await connection.QuerySingleAsync<int>(sql);

            return Result<DashboardCountDto>.Success(
                new DashboardCountDto
                {
                    Count = count
                });
        }
        catch (Exception ex)
        {
            return Result<DashboardCountDto>.Failure(
                $"Failed to fetch Total Registered Farmers Count: {ex.Message}");
        }
    }

    public async Task<Result<DashboardCountDto>> GetTotRegBacklogFarmersCount(
    string finYear)
    {
        try
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = """
            SELECT COUNT(DISTINCT fd.hf_id)
            FROM farmer_detail_horti fd
            INNER JOIN mas_farmer_horti mf
                ON fd.hf_id = mf.hf_id
            INNER JOIN equipment_details_govt_schemes e
                ON e.fd_id = fd.fd_id
            INNER JOIN view_all_villages v
                ON v.village_code = e.village_code
            WHERE e.financial_year < @FinYear
            """;

            var count = await connection.QuerySingleAsync<int>(
                sql,
                new { FinYear = finYear });

            return Result<DashboardCountDto>.Success(
                new DashboardCountDto
                {
                    Count = count
                });
        }
        catch (Exception ex)
        {
            return Result<DashboardCountDto>.Failure(
                $"Failed to fetch Total Registered Backlog Farmers Count: {ex.Message}");
        }
    }

    public async Task<Result<ApplicationDashboardDto>> GetApplicationDashboard(string finYear)
    {
        try
        {
            using var connection =
                dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = """
            SELECT
                COUNT(fa.application_id) AS TotalApplications,

                COUNT(
                    IF(fa.rheo_approval_status = 1, 1, NULL)
                ) AS TotalRheoApproved,

                COUNT(
                    IF(fa.ddh_approval_status = 1, 1, NULL)
                ) AS TotalDdhApproved,

                COUNT(
                    IF(s.scheme_type = 1, 1, NULL)
                ) AS TotalStateSponsored,

                COUNT(
                    IF(s.scheme_type = 2, 1, NULL)
                ) AS TotalCentralSponsored

            FROM temp_scheme_details s

            INNER JOIN temp_farmer_application fa
                ON s.application_id = fa.application_id

            INNER JOIN mas_scheme_horti ms
                ON ms.s_id = s.scheme_id

            INNER JOIN view_all_villages v
                ON v.village_code = s.village_code

            WHERE fa.status = 1
              AND fa.financial_year = @FinYear
            """;

            var result =
                await connection.QuerySingleAsync<ApplicationDashboardDto>(
                    sql,
                    new { FinYear = finYear });

            return Result<ApplicationDashboardDto>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<ApplicationDashboardDto>.Failure(
                $"Failed to fetch application dashboard: {ex.Message}");
        }
    }
}