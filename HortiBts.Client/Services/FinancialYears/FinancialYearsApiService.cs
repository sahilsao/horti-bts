using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Districts;
using HortiBts.Shared.Dtos.FinancialYear;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.FinancialYears
{
    public class FinancialYearsApiService(HttpClient http)
    {
        public async Task<Result<List<FinancialYearDto>>> GetFinancialYearsForReportAsync()
        {
            try
            {
                var response = await http.GetAsync("api/financial-years/fyears-for-report");
                return await ApiResultHelper.ReadResultAsync<List<FinancialYearDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FinancialYearDto>>.Failure($"Failed to fetch financial years: {ex.Message}");
            }
        }
    }
}
