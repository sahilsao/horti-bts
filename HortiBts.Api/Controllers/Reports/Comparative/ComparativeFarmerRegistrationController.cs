using HortiBts.Api.Repositories.Reports.Comparative;
using HortiBts.Shared.Dtos.Reports.Comparative;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.Comparative
{
    [Route("api/reports/comparative")]
    [ApiController]
    public class ComparativeFarmerRegistrationController(IComparativeFarmerRegistrationRepository repository,
        ILogger<DistwiseComparativeFarmerRegistrationDto> logger) : ControllerBase
    {
        [HttpGet("distwise-farmer-registration")]
        [EndpointSummary("Get comparative report of district-wise farmer registration")]
        [EndpointDescription("Retrieves the comparative report of district-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetComparativeRptOfDistwiseFarmerRegistrationAsync([FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfDistwiseComparativeFarmerRegistrationAsync(financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load comparative report of district-wise farmer registration.");
                return Problem(detail: "Failed to load comparative report of district-wise farmer registration.", statusCode: 500);
            }
        }
    }
}
