using HortiBts.Api.Repositories.Reports.Backlog;
using HortiBts.Shared.Dtos.Reports;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.Backlog
{
    [Route("api/reports/backlog")]
    [ApiController]
    public class BacklogFarmerRegistrationController(IBacklogFarmerRegistrationRepository repository,
        ILogger<DistwiseFarmerRegistrationDto> logger,
        ILogger<BlockwiseFarmerRegistrationDto> logger2) : ControllerBase
    {
        [HttpGet("distwise-farmer-registration")]
        [EndpointSummary("Get backlog report of district-wise farmer registration")]
        [EndpointDescription("Retrieves the backlog report of district-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfDistwiseFarmerRegistrationAsync([FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetBacklogRptOfDistwiseFarmerRegistrationAsync(financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load backlog report of district-wise farmer registration.");
                return Problem(detail: "Failed to load backlog report of district-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("blockwise-farmer-registration")]
        [EndpointSummary("Get backlog report of block-wise farmer registration")]
        [EndpointDescription("Retrieves the backlog report of block-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfBlockwiseFarmerRegistrationAsync([FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetBacklogRptOfBlockwiseFarmerRegistrationAsync(districtCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load backlog report of block-wise farmer registration.");
                return Problem(detail: "Failed to load backlog report of block-wise farmer registration.", statusCode: 500);
            }
        }
    }
}
