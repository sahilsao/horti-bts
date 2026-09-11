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
[Route("api/midh-sub-components")]
public class MIDHSubComponentController(IMIDHSubComponentRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<MIDHSubComponentController> logger) : ControllerBase
{

    [HttpGet("midh-sub-components-list")]
    [EndpointSummary("Get MIDH sub components list")]
    [EndpointDescription("Retrieves the list of available MIDH sub components ")]
    public async Task<ActionResult<IEnumerable<MIDHSubComponentDto>>> GetMIDHSubComponentsList()
    {
        try
        {
            var result = await repository.GetMIDHSubComponentListAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load midh sub component.");
            return Problem("Failed to load midh sub component.", statusCode: 500);
        }
    }
    

    [HttpPost("save-midh-sub-component-data")]
    [EndpointSummary("Save a new midh sub component")]
    [EndpointDescription("Inserts a new midh sub component record.")]
    public async Task<IActionResult> SaveMIDHSubComponentData([FromBody] AddMIDHSubComponentDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.SaveMIDHSubComponentAsync(dto, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result); // new scheme_id
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save midh sub component.");
            return Problem("Failed to save midh sub component.", statusCode: 500);
        }
    }

    [HttpPost("update-midh-sub-component-data")]
    [EndpointSummary("Update an existing midh sub component")]
    [EndpointDescription("Updates a midh sub component record.")]
    public async Task<IActionResult> UpdateMIDHSubComponentData([FromBody] AddMIDHSubComponentDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.UpdateMIDHSubComponentAsync(dto, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update midh sub component.");
            return Problem("Failed to update midh sub component.", statusCode: 500);
        }
    }

    [HttpPatch("{subComponentId}/active-flag")]
    [EndpointSummary("Toggle a midh sub component 's active flag")]
    [EndpointDescription("Toggles the active flag for a specific midh sub component .")]
    public async Task<IActionResult> UpdateMIDHSubComponentActiveFlag(int subComponentId, [FromBody] UpdateFlagDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
        try
        {
            var result = await repository.UpdateMIDHSubComponentActiveFlagAsync(subComponentId, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update active flag for midh sub component {subComponentId}", subComponentId);
            return Problem("Failed to update active flag.", statusCode: 500);
        }
    }
}
