using HortiBts.Api.Repositories.Units;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Units
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController(IUnitRepository unitRepository) : ControllerBase
    {
        [HttpGet("get-all-units")]
        [EndpointSummary("Get all units")]
        [EndpointDescription("Retrieves the list of all units available in the system.")]
        public async Task<IActionResult> GetUnits()
        {
            var result = await unitRepository.GetUnitsAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
    }
}
