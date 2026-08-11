using HortiBts.Shared.Common;
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
                var data = await http.GetFromJsonAsync<List<FinancialYearDto>>("api/financial-years/fyears-for-report");
                return Result<List<FinancialYearDto>>.Success(data ?? []);
            }
            catch (Exception ex)
            {
                return Result<List<FinancialYearDto>>.Failure($"Failed to fetch financial years: {ex.Message}");
            }
        }
    }
}
