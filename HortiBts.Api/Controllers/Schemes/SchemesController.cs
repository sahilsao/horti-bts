using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Dtos.Flag;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Schemes;

[ApiController]
[Route("api/schemes")]
public class SchemesController(ISchemeRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<SchemesController> logger) : ControllerBase
{
    // GET api/schemes/2  (2 = centrally sponsored)
    // GET api/schemes/1  (1 = state sponsored)
    [HttpGet("schemes-by-type")]
    [EndpointSummary("Get schemes by type")]
    [EndpointDescription("Retrieves the list of schemes based on the specified scheme type. Use 1 for state-sponsored schemes and 2 for centrally sponsored schemes.")]
    public async Task<ActionResult<IEnumerable<SchemeDto>>> GetByType([FromQuery] int stId)
    {
        try
        {
            var schemes = await repository.GetSchemesByTypeAsync(stId);
            return Ok(schemes);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load schemes for stId={StId}", stId);
            return Problem("Failed to load schemes.", statusCode: 500);
        }
    }

    [HttpGet("schemes-types")]
    [EndpointSummary("Get schemes types")]
    [EndpointDescription("Retrieves the list of available schemes types.")]
    public async Task<ActionResult<IEnumerable<SchemeTypeDto>>> GetSchemesTypes()
    {
        try
        {
            var result = await repository.GetSchemesTypesAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load scheme types.");
            return Problem("Failed to load scheme types.", statusCode: 500);
        }
    }

    [HttpGet("schemes-list")]
    [EndpointSummary("Get schemes list")]
    [EndpointDescription("Retrieves the list of available schemes.")]
    public async Task<ActionResult<IEnumerable<SchemeDto>>> GetSchemesList()
    {
        try
        {
            var result = await repository.GetSchemesListAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load schemes.");
            return Problem("Failed to load schemes.", statusCode: 500);
        }
    }

    [HttpPost("save-scheme-data")]
    [EndpointSummary("Save a new scheme")]
    [EndpointDescription("Inserts a new scheme record, optionally with a PDF attachment.")]
    [RequestSizeLimit(2 * 1024 * 1024 + 1024)] // 2MB file + small buffer
    public async Task<IActionResult> SaveSchemeData([FromForm] AddSchemeDto dto, [FromForm] IFormFile? file)
    {
        var fileValidation = ValidatePdf(file, required: true);
        if (fileValidation is not null)
            return BadRequest(new { error = true, message = fileValidation });

        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.SaveSchemeAsync(dto, file, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result); // new scheme_id
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save scheme.");
            return Problem("Failed to save scheme.", statusCode: 500);
        }
    }

    [HttpPost("update-scheme-data")]
    [EndpointSummary("Update an existing scheme")]
    [EndpointDescription("Updates a scheme record, optionally replacing its PDF attachment.")]
    [RequestSizeLimit(2 * 1024 * 1024 + 1024)]
    public async Task<IActionResult> UpdateSchemeData([FromForm] AddSchemeDto dto, [FromForm] IFormFile? file)
    {
        if (dto.SchemeId is null or 0)
            return BadRequest(new { error = true, message = "SchemeId is required for update." });

        var fileValidation = file is not null ? ValidatePdf(file, required: false) : null;
        if (fileValidation is not null)
            return BadRequest(new { error = true, message = fileValidation });

        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.UpdateSchemeAsync(dto, file, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update scheme.");
            return Problem("Failed to update scheme.", statusCode: 500);
        }
    }

    [HttpPatch("{id}/beneficiary-flag")]
    [EndpointSummary("Toggle a scheme's beneficiary flag")]
    [EndpointDescription("Toggles the beneficiary flag for a specific scheme.")]
    public async Task<IActionResult> UpdateBeneficiaryFlag(int id, [FromBody] UpdateFlagDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
        try
        {
            var result = await repository.UpdateBeneficiaryFlagAsync(id, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update beneficiary flag for scheme {SchemeId}", id);
            return Problem("Failed to update beneficiary flag.", statusCode: 500);
        }
    }

    [HttpPatch("{id}/active-flag")]
    [EndpointSummary("Toggle a scheme's active flag")]
    [EndpointDescription("Toggles the active flag for a specific scheme.")]
    public async Task<IActionResult> UpdateActiveFlag(int id, [FromBody] UpdateFlagDto dto)
    {
        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
        try
        {
            var result = await repository.UpdateActiveFlagAsync(id, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update active flag for scheme {SchemeId}", id);
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
