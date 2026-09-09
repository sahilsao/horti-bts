using HortiBts.Api.Repositories.FinancialYears;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.FinancialYears
{
    [Route("api/financial-years")]
    [ApiController]
    public class FinancialYearsController(IFinancialYearsRepository repository, ILogger<FinancialYearsController> logger) : ControllerBase
    {
        [HttpGet("fyears-for-report")]
        [EndpointSummary("Get financial years for reports")]
        [EndpointDescription("Retrieves the financial years available for use in reports.")]
        public async Task<IActionResult> GetForReport()
        {
            try
            {
                var result = await repository.GetFYearForReportAsync();

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving financial years for reports.");
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }
        }
    }

}
