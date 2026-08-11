using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using System.Data;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Dashboard;

public class DashboardApiService(HttpClient http)
{
    public async Task<Result<DashboardCountDto>> GetTotRheoAsync()
    {
        try
        {
            var result = await http.GetFromJsonAsync<Result<DashboardCountDto>>("api/dashboard/admin/get-tot-rheo-count");

            return result ?? Result<DashboardCountDto>.Failure("Empty response from server.");
        }
        catch (Exception ex)
        {
            return Result<DashboardCountDto>.Failure($"Failed to fetch total RHEO count: {ex.Message}");
        }
    }

    public async Task<Result<DashboardCountDto>> GetTotRegFarmersCountAsync()
    {
        try
        {
            var result = await http.GetFromJsonAsync<Result<DashboardCountDto>>("api/dashboard/admin/get-tot-reg-farmers-count");

            return result ??
                Result<DashboardCountDto>.Failure("Empty response from server.");
        }
        catch (Exception ex)
        {
            return Result<DashboardCountDto>.Failure($"Failed to fetch registered farmers count: {ex.Message}");
        }
    }

    public async Task<Result<DashboardCountDto>> GetTotRegBacklogFarmersCountAsync(string finYear)
    {
        try
        {
            var result = await http.GetFromJsonAsync<Result<DashboardCountDto>>($"api/dashboard/admin/get-tot-reg-backlog-farmers-count" + $"?finYear={Uri.EscapeDataString(finYear)}");

            return result ?? Result<DashboardCountDto>.Failure("Empty response from server.");
        }
        catch (Exception ex)
        {
            return Result<DashboardCountDto>.Failure($"Failed to fetch backlog farmers count: {ex.Message}");
        }
    }

    public async Task<Result<ApplicationDashboardDto>> GetApplicationDashboardAsync(string finYear)
    {
        try
        {
            var result =
                await http.GetFromJsonAsync<Result<ApplicationDashboardDto>>(
                    $"api/dashboard/admin/get-application-dashboard" +
                    $"?finYear={Uri.EscapeDataString(finYear)}");

            return result ??
                Result<ApplicationDashboardDto>.Failure(
                    "Empty response from server.");
        }
        catch (Exception ex)
        {
            return Result<ApplicationDashboardDto>.Failure(
                $"Failed to fetch application dashboard: {ex.Message}");
        }
    }
}

