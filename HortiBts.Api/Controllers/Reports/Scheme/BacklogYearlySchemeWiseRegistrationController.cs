using HortiBts.Api.Repositories.Reports.Scheme;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.Scheme
{
    [Route("api/reports")]
    [ApiController]
    public class BacklogYearlySchemeWiseRegistrationController(
     IBacklogYearlySchemeWiseRegistrationRepository repository,
     ILogger<BacklogYearlySchemeWiseRegistrationController> logger) : ControllerBase
    {
        [HttpGet("backlog/scheme-wise-farmer-registration")]
        public async Task<IActionResult> GetSchemeWiseFarmerRegistration([FromQuery] SchemeWiseReportFilterDto filter)
        {
            try
            {
                var result = await repository.GetYearlyRptOfSchemeWiseFarmerRegistrationAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load scheme-wise farmer count");
                return Problem("Failed to load scheme-wise farmer count.", statusCode: 500);
            }
        }
    }
}
