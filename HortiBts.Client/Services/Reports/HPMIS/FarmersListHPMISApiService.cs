using System;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.HPMIS;

namespace HortiBts.Client.Services.Reports.HPMIS;

public class FarmersListHPMISApiService(HttpClient http)
{
    public async Task<Result<List<FarmersListHPMISDto>>> GetFarmerListAsync(FarmerListQueryParams query)
    {
        if (query.Id is null)
            return Result<List<FarmersListHPMISDto>>.Failure("Id is required.");

        try
        {
            var url = $"api/reports/hpmis/farmer-list" +
                       $"?searchFlag={Uri.EscapeDataString(query.SearchFlag)}" +
                       $"&financialYear={Uri.EscapeDataString(query.FinancialYear.ToString())}" +
                       $"&id={query.Id}";

            var response = await http.GetAsync(url);
            return await ApiResultHelper.ReadResultAsync<List<FarmersListHPMISDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<FarmersListHPMISDto>>.Failure($"Failed to fetch farmer list: {ex.Message}");
        }
    }
}

public class FarmerListQueryParams
{
    public string SearchFlag { get; set; } = string.Empty; // DIST, SUBDIST, OFFICER, VILL, FARMER
    public int  FinancialYear { get; set; }
    public int? Id { get; set; }
}
