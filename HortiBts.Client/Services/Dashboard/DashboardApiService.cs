using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.FinancialYear;
using System.Data;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HortiBts.Client.Services.Dashboard;

public class DashboardApiService(HttpClient http)
{
    public async Task<Result<List<DashboardCountDto>>> GetTotRheoAsync()
    {
        try
        {
            var result =
                await http.GetFromJsonAsync<List<DashboardCountDto>>(
                    "api/dashboard/admin/get-tot-rheo-count");

            if (result == null)
                return Result<List<DashboardCountDto>>.Failure("Empty response from server.");

            return Result<List<DashboardCountDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<DashboardCountDto>>.Failure(
                $"Failed to fetch total RHEO count: {ex.Message}");
        }
    }

    public async Task<Result<List<DashboardCountDto>>> GetTotRegFarmersCountAsync()
    {
        try
        {
            var result = await http.GetFromJsonAsync<List<DashboardCountDto>>("api/dashboard/admin/get-tot-reg-farmers-count");
            
            if (result == null)
                return Result<List<DashboardCountDto>>.Failure("Empty response from server.");

            return Result<List<DashboardCountDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<DashboardCountDto>>.Failure($"Failed to fetch registered farmers count: {ex.Message}");
        }
    }

    public async Task<Result<List<DashboardCountDto>>> GetTotRegBacklogFarmersCountAsync(string finYear)
    {
        try
        {
            var result = await http.GetFromJsonAsync<List<DashboardCountDto>>($"api/dashboard/admin/get-tot-reg-backlog-farmers-count" + $"?finYear={Uri.EscapeDataString(finYear)}");

            if (result == null)
                return Result<List<DashboardCountDto>>.Failure("Empty response from server.");

            return Result<List<DashboardCountDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<DashboardCountDto>>.Failure($"Failed to fetch backlog farmers count: {ex.Message}");
        }
    }

    public async Task<Result<List<ApplicationDashboardDto>>> GetApplicationDashboardAsync(string finYear)
    {
        try
        {
            var result =
                await http.GetFromJsonAsync<List<ApplicationDashboardDto>>(
                    $"api/dashboard/admin/get-application-dashboard" +
                    $"?finYear={Uri.EscapeDataString(finYear)}");

            if (result == null)
                return Result<List<ApplicationDashboardDto>>.Failure("Empty response from server.");

            return Result<List<ApplicationDashboardDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<ApplicationDashboardDto>>.Failure(
                $"Failed to fetch application dashboard: {ex.Message}");
        }
    }
}

