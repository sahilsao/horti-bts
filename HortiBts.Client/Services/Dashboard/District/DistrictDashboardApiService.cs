using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.District;
namespace HortiBts.Client.Services.Dashboard.District;

public class DistrictDashboardApiService(HttpClient http)
{
    public async Task<Result<List<DistrictDashboardCountDto>>> GetTotRheoAsync(int districtCode)
    {
        try
        {
            var response = await http.GetAsync($"api/dashboard/district/get-tot-rheo-count?districtCode={districtCode}");
            return await ApiResultHelper.ReadResultAsync<List<DistrictDashboardCountDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<DistrictDashboardCountDto>>.Failure($"Failed to fetch total RHEO count: {ex.Message}");
        }
    }

    public async Task<Result<List<DistrictDashboardCountDto>>> GetTotRegFarmersCountAsync(int districtCode)
    {
        try
        {
            var response = await http.GetAsync($"api/dashboard/district/get-tot-reg-farmers-count?districtCode={districtCode}");
            return await ApiResultHelper.ReadResultAsync<List<DistrictDashboardCountDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<DistrictDashboardCountDto>>.Failure($"Failed to fetch registered farmers count: {ex.Message}");
        }
    }

    public async Task<Result<List<DistrictDashboardCountDto>>> GetTotRegBacklogFarmersCountAsync(string finYear, int districtCode)
    {
        try
        {
            var response = await http.GetAsync($"api/dashboard/district/get-tot-reg-backlog-farmers-count?finYear={Uri.EscapeDataString(finYear)}&districtCode={districtCode}");
            return await ApiResultHelper.ReadResultAsync<List<DistrictDashboardCountDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<DistrictDashboardCountDto>>.Failure($"Failed to fetch backlog farmers count: {ex.Message}");
        }
    }

    public async Task<Result<List<DistrictApplicationDashboardDto>>> GetApplicationDashboardAsync(string finYear, int districtCode)
    {
        try
        {
            var response =
                await http.GetAsync($"api/dashboard/district/get-application-dashboard" + 
                $"?finYear={Uri.EscapeDataString(finYear)}&districtCode={districtCode}");
            return await ApiResultHelper.ReadResultAsync<List<DistrictApplicationDashboardDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<DistrictApplicationDashboardDto>>.Failure(
                $"Failed to fetch application dashboard: {ex.Message}");
        }
    }
}

