using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard;
using HortiBts.Shared.Dtos.FinancialYear;

namespace HortiBts.Api.Repositories.FinancialYears
{
    public interface IFinancialYearsRepository
    {
        /// <summary>Ported from commonservice.js getFYearForReport.</summary>
        Task<Result<List<FinancialYearDto>>> GetFYearForReportAsync();
    }

    public class FinancialYearsRepository(IDbConnectionFactory dbFactory) : IFinancialYearsRepository
    {
        public async Task<Result<List<FinancialYearDto>>> GetFYearForReportAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    select id as Id, financial_year as FinancialYear
                    from mas_financialyear
                    where id > 8
                    order by id desc
                    """;
                var result = await connection.QueryAsync<FinancialYearDto>(sql);
                return Result<List<FinancialYearDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<FinancialYearDto>>.Failure($"Failed to fetch financial years: {ex.Message}");
            }
        }
    }
}
