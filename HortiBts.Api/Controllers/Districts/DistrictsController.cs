using HortiBts.Api.Repositories.Districts;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Districts
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController(IDistrictsRepository districtRepository, ILogger<DistrictsController> logger) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all districts")]
        [EndpointDescription("Retrieves the list of all districts available in the system.")]
        public async Task<IActionResult> GetDistricts()
        {
            try
            {
                var result = await districtRepository.GetDistrictsAsync();

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving districts.");
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }
        }
    }
}
