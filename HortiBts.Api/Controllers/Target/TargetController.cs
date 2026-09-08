using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Target;
using HortiBts.Shared.Dtos.Target;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Target
{
    [Route("api/target")]
    [ApiController]
    public class TargetController(ITargetRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<TargetController> logger) : ControllerBase
    {
        [HttpGet("get-user-targets-list")]
        [EndpointSummary("Get targets list")]
        [EndpointDescription("Retrieves the list of available targets.")]
        public async Task<ActionResult<IEnumerable<TargetDto>>> GetTargetsList()
        {
            try
            {
                var result = await repository.GetTargetListAsync();

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to load targets.");

                return Problem(
                    "Failed to load targets.",
                    statusCode: 500);
            }
        }

        [HttpPost("save-target-data")]
        [EndpointSummary("Save a new target")]
        [EndpointDescription("Inserts a new target record.")]
        public async Task<IActionResult> SaveTarget([FromBody] AddTargetDto dto)
        {
            var userId = User.Identity?.Name ?? "0";
            var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

            try
            {
                var result = await repository.SaveTargetAsync(dto, userId, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save target.");
                return Problem("Failed to save target.", statusCode: 500);
            }
        }
    }
}
