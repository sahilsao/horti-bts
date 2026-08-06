using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Dashboard;

public class DashboardApiService(HttpClient http)
{
    // ---- READY: backed by a real, ported endpoint (see FinancialYearsController) ----
    public async Task<Result<List<FinancialYearDto>>> GetFinancialYearsForReportAsync()
    {
        try
        {
            var data = await http.GetFromJsonAsync<List<FinancialYearDto>>("api/financial-years/for-report");
            return Result<List<FinancialYearDto>>.Success(data ?? []);
        }
        catch (Exception ex)
        {
            return Result<List<FinancialYearDto>>.Failure($"Failed to fetch financial years: {ex.Message}");
        }
    }

    // ---- PENDING: endpoint contract only — API side needs dashboardRoutes.js / dashboardservice.js ----

    /// <summary>Mirrors GET /dashboard/getRAEOCount_RHEO/:id/:type</summary>
    public async Task<Result<int>> GetRaeoCountRheoAsync(int id, string type)
    {
        try
        {
            var data = await http.GetFromJsonAsync<List<RaeoCountRow>>($"api/dashboard/raeo-count-rheo/{id}/{type}");
            return Result<int>.Success(data?.FirstOrDefault()?.TotalCount ?? 0);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Failed to fetch RHEO count: {ex.Message}");
        }
    }

    /// <summary>Mirrors GET /dashboard/getTotalHortiFrmrStatusCount/:statusFlag/:id/:type</summary>
    public async Task<Result<int>> GetTotalHortiFarmerStatusCountAsync(int statusFlag, int id, string type)
    {
        try
        {
            var data = await http.GetFromJsonAsync<List<RaeoCountRow>>(
                $"api/dashboard/farmer-status-count/{statusFlag}/{id}/{type}");
            return Result<int>.Success(data?.FirstOrDefault()?.TotalCount ?? 0);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Failed to fetch farmer status count: {ex.Message}");
        }
    }

    /// <summary>Mirrors GET /dashboardRoutes/getDashboardAppCount?financial_year&amp;id&amp;type</summary>
    public async Task<Result<FarmerCountsDto>> GetDashboardAppCountAsync(int financialYearId, int id, string type)
    {
        try
        {
            var data = await http.GetFromJsonAsync<FarmerCountsDto>(
                $"api/dashboard/app-count?financialYear={financialYearId}&id={id}&type={type}");
            return Result<FarmerCountsDto>.Success(data ?? new FarmerCountsDto());
        }
        catch (Exception ex)
        {
            return Result<FarmerCountsDto>.Failure($"Failed to fetch dashboard app count: {ex.Message}");
        }
    }

    /// <summary>
    /// Mirrors the three parallel calls in getMainDashboard() — getDashboardDataAllF /
    /// BacklogF / AppF — bundled into a single endpoint here since they share the same
    /// (id, financialYear, searchFlag) params. If your API keeps them as three separate
    /// routes, split this back into three calls with Task.WhenAll instead.
    /// </summary>
    public async Task<Result<DashboardMainDto>> GetDashboardMainAsync(int id, int financialYearId, string searchFlag)
    {
        try
        {
            var data = await http.GetFromJsonAsync<DashboardMainDto>(
                $"api/dashboard/main?id={id}&financialYear={financialYearId}&searchFlag={searchFlag}");
            return Result<DashboardMainDto>.Success(data ?? new DashboardMainDto());
        }
        catch (Exception ex)
        {
            return Result<DashboardMainDto>.Failure($"Failed to fetch dashboard summary: {ex.Message}");
        }
    }

    private class RaeoCountRow
    {
        public int TotalCount { get; set; }
    }
}