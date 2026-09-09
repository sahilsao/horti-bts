using HortiBts.Api.Repositories.Villages;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Villages
{
    [Route("api/get-villages-from-sub-district")]
    [ApiController]
    public class VillagesController(IVillagesRepository villagesRepository, ILogger<VillagesController> logger) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all villages from selected sub-district")]
        [EndpointDescription("Retrieves the list of all villages of the selected sub-district available in the system.")]
        public async Task<IActionResult> GetVillages([FromQuery] string SubDistrictCode)
        {
            try
            {
                var result = await villagesRepository.GetVillagesAsync(SubDistrictCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving villages for sub-district code: {SubDistrictCode}", SubDistrictCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
