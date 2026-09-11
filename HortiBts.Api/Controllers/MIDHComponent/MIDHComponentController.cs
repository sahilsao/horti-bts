using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.MIDHSchemes;
using HortiBts.Shared.Dtos.Flag;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;
using HortiBts.Shared.Dtos.MIDHSchemes;
using HortiBts.Api.Repositories.MIDHComponents;
using HortiBts.Shared.Dtos.MIDHComponents;

namespace HortiBts.Api.Controllers.MIDHComponent;

[ApiController]
[Route("api/midh-components")]
public class MIDHComponentController(IMIDHComponentRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<MIDHComponentController> logger) : ControllerBase
{

    [HttpGet("midh-components-list")]
    [EndpointSummary("Get MIDH components list")]
    [EndpointDescription("Retrieves the list of available MIDH components ")]
    public async Task<ActionResult<IEnumerable<MIDHComponentDto>>> GetMIDHComponentsList()
    {
        try
        {
            var result = await repository.GetMIDHComponentListAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load midh component.");
            return Problem("Failed to load midh component.", statusCode: 500);
        }
    }

    [HttpGet("midh-components-list-by-component-id")]
    [EndpointSummary("Get MIDH components list by component id")]
    [EndpointDescription("Retrieves the list of available MIDH components for a specific component.")]
    public async Task<ActionResult<IEnumerable<MIDHComponentDto>>> GetMIDHComponentsListByComponentId([FromQuery] int componentId)
    {
        try
        {
            var result = await repository.GetMIDHComponentListByComponentIdAsync(componentId);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load midh component.");
            return Problem("Failed to load midh component.", statusCode: 500);
        }
    }

    [HttpGet("midh-components-list-by-component-type-id")]
    [EndpointSummary("Get MIDH components list by component type id")]
    [EndpointDescription("Retrieves the list of available MIDH components for a specific component type.")]
    public async Task<ActionResult<IEnumerable<MIDHComponentDto>>> GetMIDHComponentsListByComponentTypeId([FromQuery] int componentTypeId)
    {
        try
        {
            var result = await repository.GetMIDHComponentListByComponentTypeIdAsync(componentTypeId);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load midh component.");
            return Problem("Failed to load midh component.", statusCode: 500);
        }
    }

    [HttpPost("save-midh-component-data")]
    [EndpointSummary("Save a new midh component")]
    [EndpointDescription("Inserts a new midh component record.")]
    public async Task<IActionResult> SaveMIDHComponentData([FromBody] AddMIDHComponentDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.SaveMIDHComponentAsync(dto, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result); // new scheme_id
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save midh component.");
            return Problem("Failed to save midh component.", statusCode: 500);
        }
    }

    [HttpPost("update-midh-component-data")]
    [EndpointSummary("Update an existing midh component")]
    [EndpointDescription("Updates a midh component record.")]
    public async Task<IActionResult> UpdateMIDHComponentData([FromBody] AddMIDHComponentDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.UpdateMIDHComponentAsync(dto, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update midh scheme.");
            return Problem("Failed to update midh scheme.", statusCode: 500);
        }
    }

    [HttpPatch("{componentId}/active-flag")]
    [EndpointSummary("Toggle a midh component 's active flag")]
    [EndpointDescription("Toggles the active flag for a specific midh component .")]
    public async Task<IActionResult> UpdateMIDHComponentActiveFlag(int componentId, [FromBody] UpdateFlagDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
        try
        {
            var result = await repository.UpdateMIDHComponentActiveFlagAsync(componentId, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update active flag for midh component {componentId}", componentId);
            return Problem("Failed to update active flag.", statusCode: 500);
        }
    }
}
