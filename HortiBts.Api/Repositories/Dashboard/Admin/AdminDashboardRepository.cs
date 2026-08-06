using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;

namespace HortiBts.Api.Repositories.Dashboard.Admin
{
    public interface IDashboardRepository
    {
        Task<Result<List<DashboardDto>>> GetTotRHEOCount();
        Task<Result<List<DashboardDto>>> GetTotRegFarmersCount();
        Task<Result<List<DashboardDto>>> GetTotRegBacklogFarmersCount(string finYear);
    }
    public class AdminDashboardRepository(IDbConnectionFactory dbFactory) : IDashboardRepository
    {
        public async Task<Result<List<DashboardDto>>> GetTotRHEOCount()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    select count(1) as TotRHEO from mas_raeo where usertype=3
                    """;
                var result = await connection.QuerySingleAsync<DashboardDto>(sql);
                return Result<List<DashboardDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<DashboardDto>>.Failure($"Failed to fetch Total RHEO Count: {ex.Message}");
            }

        }

        public async Task<Result<List<DashboardDto>>> GetTotRegFarmersCount()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    SELECT COUNT(DISTINCT fd.hf_id) as TotRegFarmers FROM farmer_detail_horti fd 
                    INNER JOIN mas_farmer_horti mf ON fd.hf_id=mf.hf_id
                    INNER JOIN equipment_details_govt_schemes e ON e.fd_id=fd.fd_id 
                    INNER JOIN view_all_villages v ON v.village_code=e.village_code 
                    """;
                var result = await connection.QueryAsync<DashboardDto>(sql);
                return Result<List<DashboardDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<DashboardDto>>.Failure($"Failed to fetch Total Registered Farmers Count: {ex.Message}");
            }

        }

        public async Task<Result<List<DashboardDto>>> GetTotRegBacklogFarmersCount(string finYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    SELECT COUNT(DISTINCT fd.hf_id) as TotRegBacklogFarmers FROM farmer_detail_horti fd 
                    INNER JOIN mas_farmer_horti mf ON fd.hf_id=mf.hf_id
                    INNER JOIN equipment_details_govt_schemes e ON e.fd_id=fd.fd_id 
                    INNER JOIN view_all_villages v ON v.village_code=e.village_code where e.financial_year < @FinYear
                    """;
                var result = await connection.QueryAsync<DashboardDto>(sql, new { FinYear = finYear });
                return Result<List<DashboardDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<DashboardDto>>.Failure($"Failed to fetch Total Registered Backlog Farmers Count: {ex.Message}");
            }

        }

    }
}
