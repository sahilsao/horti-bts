using HortiBts.Api.Repositories.SubDistricts;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.SubDistricts
{
    [Route("api/get-sub-districts-from-district")]
    [ApiController]
    public class SubDistrictsController(ISubDistrictsRepository subDistrictRepository, ILogger<SubDistrictsController> logger) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all sub-districts from selected district")]
        [EndpointDescription("Retrieves the list of all sub-districts of the selected district available in the system.")]
        public async Task<IActionResult> GetSubDistricts([FromQuery] string DistrictCode)
        {
            try
            {
                var result = await subDistrictRepository.GetSubDistrictsAsync(DistrictCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving sub-districts for district code: {DistrictCode}", DistrictCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
