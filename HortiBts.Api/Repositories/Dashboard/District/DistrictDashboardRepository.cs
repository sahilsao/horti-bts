using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.District;

namespace HortiBts.Api.Repositories.Dashboard.District;

public interface IDistrictDashboardRepository
{
    Task<Result<List<DistrictDashboardCountDto>>> GetTotRHEOCount(int districtCode);

    Task<Result<List<DistrictDashboardCountDto>>> GetTotRegFarmersCount(int districtCode);

    Task<Result<List<DistrictDashboardCountDto>>> GetTotRegBacklogFarmersCount(string finYear, int districtCode);

    Task<Result<List<DistrictApplicationDashboardDto>>> GetApplicationDashboard(string finYear, int districtCode);
}
public class DistrictDashboardRepository(IDbConnectionFactory dbFactory) : IDistrictDashboardRepository
{
    public async Task<Result<List<DistrictDashboardCountDto>>> GetTotRHEOCount(int districtCode)
    {
        try
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);

            const string sql = """
            SELECT COUNT(1)
            FROM mas_raeo
            WHERE usertype = 3 AND district_code = @DistrictCode
            """;

            var count = await connection.QuerySingleAsync<int>(sql, new { DistrictCode = districtCode });

            return Result<List<DistrictDashboardCountDto>>.Success(
                new List<DistrictDashboardCountDto>
                {
                    new DistrictDashboardCountDto
                    {
                        Count = count
                    }
                });
        }
        catch (Exception ex)
        {
            return Result<List<DistrictDashboardCountDto>>.Failure(
                $"Failed to fetch Total RHEO Count: {ex.Message}");
        }
    }

    public async Task<Result<List<DistrictDashboardCountDto>>> GetTotRegFarmersCount(int districtCode)
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
                ON v.village_code = e.village_code  WHERE fd.district_census = @DistrictCode
            """;

            var count = await connection.QuerySingleAsync<int>(sql, new { DistrictCode = districtCode });

            return Result<List<DistrictDashboardCountDto>>.Success(
                new List<DistrictDashboardCountDto>
                {
                    new DistrictDashboardCountDto
                    {
                        Count = count
                    }
                });
        }
        catch (Exception ex)
        {
            return Result<List<DistrictDashboardCountDto>>.Failure(
                $"Failed to fetch Total Registered Farmers Count: {ex.Message}");
        }
    }

    public async Task<Result<List<DistrictDashboardCountDto>>> GetTotRegBacklogFarmersCount(
    string finYear, int districtCode)
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
            WHERE e.financial_year < @FinYear AND fd.district_census = @DistrictCode
            """;

            var count = await connection.QuerySingleAsync<int>(
                sql,
                new { FinYear = finYear, DistrictCode = districtCode });

            return Result<List<DistrictDashboardCountDto>>.Success(
                new List<DistrictDashboardCountDto>
                {
                    new DistrictDashboardCountDto
                    {
                        Count = count
                    }
                });
        }
        catch (Exception ex)
        {
            return Result<List<DistrictDashboardCountDto>>.Failure(
                $"Failed to fetch Total Registered Backlog Farmers Count: {ex.Message}");
        }
    }

    public async Task<Result<List<DistrictApplicationDashboardDto>>> GetApplicationDashboard(string finYear, int districtCode)
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
              AND fa.financial_year = @FinYear AND fa.district_code = @DistrictCode
            """;

            var result =
                await connection.QuerySingleAsync<DistrictApplicationDashboardDto>(
                    sql,
                    new { FinYear = finYear, DistrictCode = districtCode });

            return Result<List<DistrictApplicationDashboardDto>>.Success(new List<DistrictApplicationDashboardDto> { result });
        }
        catch (Exception ex)
        {
            return Result<List<DistrictApplicationDashboardDto>>.Failure(
                $"Failed to fetch application dashboard: {ex.Message}");
        }
    }
}