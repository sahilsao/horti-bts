using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
namespace HortiBts.Client.Services.Dashboard.Admin;

public class AdminDashboardApiService(HttpClient http)
{
    public async Task<Result<List<AdminDashboardCountDto>>> GetTotRheoAsync()
    {
        try
        {
            var response = await http.GetAsync("api/dashboard/admin/get-tot-rheo-count");
            return await ApiResultHelper.ReadResultAsync<List<AdminDashboardCountDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<AdminDashboardCountDto>>.Failure($"Failed to fetch total RHEO count: {ex.Message}");
        }
    }

    public async Task<Result<List<AdminDashboardCountDto>>> GetTotRegFarmersCountAsync()
    {
        try
        {
            var response = await http.GetAsync("api/dashboard/admin/get-tot-reg-farmers-count");
            return await ApiResultHelper.ReadResultAsync<List<AdminDashboardCountDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<AdminDashboardCountDto>>.Failure($"Failed to fetch registered farmers count: {ex.Message}");
        }
    }

    public async Task<Result<List<AdminDashboardCountDto>>> GetTotRegBacklogFarmersCountAsync(string finYear)
    {
        try
        {
            var response = await http.GetAsync($"api/dashboard/admin/get-tot-reg-backlog-farmers-count?finYear={Uri.EscapeDataString(finYear)}");
            return await ApiResultHelper.ReadResultAsync<List<AdminDashboardCountDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<AdminDashboardCountDto>>.Failure($"Failed to fetch backlog farmers count: {ex.Message}");
        }
    }

    public async Task<Result<List<AdminApplicationDashboardDto>>> GetApplicationDashboardAsync(string finYear)
    {
        try
        {
            var response =
                await http.GetAsync($"api/dashboard/admin/get-application-dashboard" + 
                $"?finYear={Uri.EscapeDataString(finYear)}");
            return await ApiResultHelper.ReadResultAsync<List<AdminApplicationDashboardDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<AdminApplicationDashboardDto>>.Failure(
                $"Failed to fetch application dashboard: {ex.Message}");
        }
    }
}

