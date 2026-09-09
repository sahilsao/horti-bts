using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.MIDHSchemes;
using HortiBts.Shared.Dtos.Flag;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;
using HortiBts.Shared.Dtos.MIDHSchemes;

namespace HortiBts.Api.Controllers.MIDHSchemes;

[ApiController]
[Route("api/midh-schemes")]
public class MIDHSchemesController(IMIDHSchemeRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<MIDHSchemesController> logger) : ControllerBase
{

    [HttpGet("midh-schemes-list")]
    [EndpointSummary("Get MIDH schemes list")]
    [EndpointDescription("Retrieves the list of available MIDH schemes.")]
    public async Task<ActionResult<IEnumerable<SchemeDto>>> GetMIDHSchemesList()
    {
        try
        {
            var result = await repository.GetMIDHSchemesListAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to load schemes.");

            return Problem(
                "Failed to load schemes.",
                statusCode: 500);
        }
    }

    [HttpGet("midh-schemes-list-by-scheme-id")]
    [EndpointSummary("Get MIDH schemes list by scheme type")]
    [EndpointDescription("Retrieves the list of available MIDH schemes for a specific scheme type.")]
    public async Task<ActionResult<IEnumerable<MIDHSchemeDto>>> GetMIDHSchemesListBySchemeType([FromQuery] int schemeTypeId)
    {
        try
        {
            var result = await repository.GetMIDHSchemesBySchemeIDAsync(schemeTypeId);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to load schemes.");

            return Problem(
                "Failed to load schemes.",
                statusCode: 500);
        }
    }

    [HttpPost("save-midh-scheme-data")]
    [EndpointSummary("Save a new midh scheme")]
    [EndpointDescription("Inserts a new midh scheme record, optionally with a PDF attachment.")]
    [RequestSizeLimit(2 * 1024 * 1024 + 1024)] // 2MB file + small buffer
    public async Task<IActionResult> SaveMIDHSchemeData([FromForm] AddMIDHSchemeDto dto, [FromForm] IFormFile? file)
    {
        var fileValidation = ValidatePdf(file, required: true);
        if (fileValidation is not null)
            return BadRequest(new { error = true, message = fileValidation });

        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.SaveMIDHSchemeAsync(dto, file, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result); // new scheme_id
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save midh scheme.");
            return Problem("Failed to save midh scheme.", statusCode: 500);
        }
    }

    [HttpPost("update-midh-scheme-data")]
    [EndpointSummary("Update an existing midh scheme")]
    [EndpointDescription("Updates a midh scheme record, optionally replacing its PDF attachment.")]
    [RequestSizeLimit(2 * 1024 * 1024 + 1024)]
    public async Task<IActionResult> UpdateMIDHSchemeData([FromForm] AddMIDHSchemeDto dto, [FromForm] IFormFile? file)
    {
        if (dto.MIDHSchemeId is null or 0)
            return BadRequest(new { error = true, message = "MIDHSchemeId is required for update." });

        var fileValidation = file is not null ? ValidatePdf(file, required: false) : null;
        if (fileValidation is not null)
            return BadRequest(new { error = true, message = fileValidation });

        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.UpdateMIDHSchemeAsync(dto, file, userId, clientIp);

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

    [HttpPatch("{schemeId}/active-flag")]
    [EndpointSummary("Toggle a midh scheme's active flag")]
    [EndpointDescription("Toggles the active flag for a specific midh scheme.")]
    public async Task<IActionResult> UpdateMIDHSchemeActiveFlag(int schemeId, [FromBody] UpdateFlagDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
        try
        {
            var result = await repository.UpdateMIDHActiveFlagAsync(schemeId, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update active flag for midh scheme {schemeId}", schemeId);
            return Problem("Failed to update active flag.", statusCode: 500);
        }
    }

    private static string? ValidatePdf(IFormFile? file, bool required)
    {
        if (file is null)
            return required ? "PDF file is required." : null;

        const long maxBytes = 2 * 1024 * 1024;

        if (file.Length == 0 || file.Length > maxBytes)
            return "File size invalid or exceeds allowed limit.";

        if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            return "Only PDF files are allowed.";

        using var stream = file.OpenReadStream();
        var header = new byte[5];
        stream?.Read(header, 0, 5);
        stream?.Position = 0;

        var isPdf = header.Length >= 5 &&
                    header[0] == '%' && header[1] == 'P' && header[2] == 'D' && header[3] == 'F' && header[4] == '-';

        return isPdf ? null : "Uploaded file is not a valid PDF.";
    }
}
