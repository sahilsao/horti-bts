using HortiBts.Api.Repositories.Reports.Component;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.Component
{
    [Route("api/reports")]
    [ApiController]
    public class BacklogYearlyComponentWiseRegistrationController(
     IBacklogYearlyComponentWiseRegistrationRepository repository,
     ILogger<BacklogYearlyComponentWiseRegistrationController> logger) : ControllerBase
    {
        [HttpGet("backlog/component-wise-farmer-registration")]
        public async Task<IActionResult> GetComponentWiseFarmerRegistration([FromQuery] ComponentWiseReportFilterDto filter)
        {
            try
            {
                var result = await repository.GetYearlyRptOfComponentWiseFarmerRegistrationAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load component-wise farmer count");
                return Problem("Failed to load component-wise farmer count.", statusCode: 500);
            }
        }
    }
}
