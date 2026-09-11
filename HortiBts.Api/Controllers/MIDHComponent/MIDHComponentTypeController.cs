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
[Route("api/midh-components-types")]
public class MIDHComponentTypesController(IMIDHComponentTypeRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<MIDHComponentTypesController> logger) : ControllerBase
{

    [HttpGet("midh-components-type-list")]
    [EndpointSummary("Get MIDH components types list")]
    [EndpointDescription("Retrieves the list of available MIDH components types.")]
    public async Task<ActionResult<IEnumerable<MIDHComponentTypeDto>>> GetMIDHSchemesList()
    {
        try
        {
            var result = await repository.GetMIDHComponentTypeListAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load midh component types.");
            return Problem("Failed to load midh component types.", statusCode: 500);
        }
    }

    [HttpGet("midh-components-type-list-by-scheme-id")]
    [EndpointSummary("Get MIDH components types list by scheme id")]
    [EndpointDescription("Retrieves the list of available MIDH components types for a specific scheme.")]
    public async Task<ActionResult<IEnumerable<MIDHComponentTypeDto>>> GetMIDHSchemesListBySchemeId([FromQuery] int schemeId)
    {
        try
        {
            var result = await repository.GetMIDHComponentTypeListBySchemeIdAsync(schemeId);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load midh component types.");
            return Problem("Failed to load midh component types.", statusCode: 500);
        }
    }

    [HttpPost("save-midh-component-type-data")]
    [EndpointSummary("Save a new midh component type")]
    [EndpointDescription("Inserts a new midh component type record.")]
    public async Task<IActionResult> SaveMIDHComponentTypeData([FromBody] AddMIDHComponentTypeDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.SaveMIDHComponentTypeAsync(dto, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result); // new scheme_id
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save midh component type.");
            return Problem("Failed to save midh component type.", statusCode: 500);
        }
    }

    [HttpPost("update-midh-component-type-data")]
    [EndpointSummary("Update an existing midh component type")]
    [EndpointDescription("Updates a midh component type record.")]
    public async Task<IActionResult> UpdateMIDHComponentTypeData([FromBody] AddMIDHComponentTypeDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.UpdateMIDHComponentTypeAsync(dto, userId, clientIp);

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

    [HttpPatch("{componentTypeId}/active-flag")]
    [EndpointSummary("Toggle a midh component type's active flag")]
    [EndpointDescription("Toggles the active flag for a specific midh component type.")]
    public async Task<IActionResult> UpdateMIDHComponentTypeActiveFlag(int componentTypeId, [FromBody] UpdateFlagDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
        try
        {
            var result = await repository.UpdateMIDHComponentTypeActiveFlagAsync(componentTypeId, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update active flag for midh component type {componentTypeId}", componentTypeId);
            return Problem("Failed to update active flag.", statusCode: 500);
        }
    }
}
