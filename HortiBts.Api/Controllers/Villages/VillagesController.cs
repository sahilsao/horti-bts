using HortiBts.Api.Repositories.Villages;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Villages
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillagesController(IVillagesRepository villagesRepository, ILogger<VillagesController> logger) : ControllerBase
    {
        [HttpGet("get-villages-from-sub-district")]
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

        [HttpGet("get-villages-for-rheo")]
        [EndpointSummary("Get all villages for rheo from selected sub-district")]
        [EndpointDescription("Retrieves the list of all villages for rheo of the selected sub-district available in the system.")]
        public async Task<IActionResult> GetVillagesForRheo([FromQuery] int DepartmentCode, [FromQuery] string SubDistrictCode)
        {
            try
            {
                var result = await villagesRepository.GetVillagesForRHEOAsync(DepartmentCode, SubDistrictCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving villages for rheo for sub-district code: {SubDistrictCode}", SubDistrictCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
