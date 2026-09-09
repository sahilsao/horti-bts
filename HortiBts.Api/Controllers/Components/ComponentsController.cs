using HortiBts.Api.Controllers.Schemes;
using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Benefits;
using HortiBts.Api.Repositories.Components;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Flag;
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

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load components.");

                return Problem("Failed to load components.", statusCode: 500);
            }
        }

        [HttpPost("save-component-data")]
        [EndpointSummary("Save a new component")]
        [EndpointDescription("Inserts a new component record.")]
        public async Task<IActionResult> SaveComponent([FromBody] AddComponentDto dto)
        {
            try
            {
                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
                var result = await repository.SaveComponentAsync(dto, userId, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save component.");
                return Problem(detail: "Failed to save component.", statusCode: 500);
            }
        }

        [HttpPost("update-component-data")]
        [EndpointSummary("Update an existing component")]
        [EndpointDescription("Updates an existing component record.")]
        public async Task<IActionResult> UpdateComponent([FromBody] AddComponentDto dto)
        {
            try
            {
                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await repository.UpdateComponentAsync(dto, userId, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update component.");
                return Problem(detail: "Failed to update component.", statusCode: 500);
            }
        }

        [HttpPatch("{id}/flag")]
        [EndpointSummary("Toggle a benefit's active status")]
        public async Task<IActionResult> UpdateFlag(int id, [FromBody] UpdateFlagDto dto)
        {
            try
            {
                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await repository.UpdateComponentActiveFlagAsync(id, dto.Flag, userId, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update component flag.");
                return Problem(detail: "Failed to update component flag.", statusCode: 500);
            }
        }
    }
}

