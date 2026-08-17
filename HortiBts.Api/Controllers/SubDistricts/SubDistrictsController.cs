using HortiBts.Api.Repositories.SubDistricts;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.SubDistricts
{
    [Route("api/get-sub-districts-from-district")]
    [ApiController]
    public class SubDistrictsController(ISubDistrictsRepository subDistrictRepository) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all sub-districts from selected district")]
        [EndpointDescription("Retrieves the list of all sub-districts of the selected district available in the system.")]
        public async Task<IActionResult> GetSubDistricts([FromQuery] string DistrictCode)
        {
            var result = await subDistrictRepository.GetSubDistrictsAsync(DistrictCode);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result.Data);
        }
    }
}
