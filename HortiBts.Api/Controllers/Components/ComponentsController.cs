using HortiBts.Api.Controllers.Schemes;
using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Components;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Components
{
    [Route("api/components")]
    [ApiController]
    public class ComponentsController(IComponentRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<ComponentsController> logger) : ControllerBase
    {
        [HttpGet("components-list")]
        [EndpointSummary("Get components list")]
        [EndpointDescription("Retrieves the list of available components.")]
        public async Task<ActionResult<IEnumerable<ComponentDto>>> GetComponentsList()
        {
            try
            {
                var result = await repository.GetComponentListAsync();

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to load components.");

                return Problem(
                    "Failed to load components.",
                    statusCode: 500);
            }
        }

        [HttpPost("save-component-data")]
        [EndpointSummary("Save a new component")]
        [EndpointDescription("Inserts a new component record.")]
        public async Task<IActionResult> SaveComponent([FromForm] AddComponentDto dto)
        {
            var userId = User.Identity?.Name ?? "system";
            var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

            try
            {
                var result = await repository.SaveComponentAsync(dto, userId, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save component.");
                return Problem("Failed to save component.", statusCode: 500);
            }
        }
    }
}

           