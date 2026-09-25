using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Notices;
using HortiBts.Shared.Dtos.Flag;
using HortiBts.Shared.Dtos.Notices;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers;

[ApiController]
[Route("api/notices")]
public class NoticesController(INoticeRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<NoticesController> logger) : ControllerBase
{

    // GET api/notices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoticesDto>>> GetAll()
    {
        try
        {
            var notices = await repository.GetActiveAsync();
            return Ok(notices);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load notices");
            return Problem("Failed to load notices.", statusCode: 500);
        }
    }

    [HttpPost("save-notice-data")]
    [EndpointSummary("Save a new notice")]
    [EndpointDescription("Inserts a new notice record, optionally with a PDF attachment.")]
    [RequestSizeLimit(2 * 1024 * 1024 + 1024)] // 2MB file + small buffer
    public async Task<IActionResult> SaveSchemeData([FromForm] AddNoticesDto dto, [FromForm] IFormFile? file)
    {
        var fileValidation = ValidatePdf(file, required: true);
        if (fileValidation is not null)
            return BadRequest(new { error = true, message = fileValidation });

        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.SaveNoticeAsync(dto, file, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result); // new notice_id
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save notice.");
            return Problem("Failed to save notice.", statusCode: 500);
        }
    }

    [HttpPost("update-notice-data")]
    [EndpointSummary("Update an existing notice")]
    [EndpointDescription("Updates a notice record, optionally replacing its PDF attachment.")]
    [RequestSizeLimit(2 * 1024 * 1024 + 1024)]
    public async Task<IActionResult> UpdateNoticeData([FromForm] AddNoticesDto dto, [FromForm] IFormFile? file)
    {
        if (dto.NoticeId == 0)
            return BadRequest(new { error = true, message = "NoticeeId is required for update." });

        var fileValidation = file is not null ? ValidatePdf(file, required: false) : null;
        if (fileValidation is not null)
            return BadRequest(new { error = true, message = fileValidation });

        var userId = User.Identity?.Name ?? "0";
        var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

        try
        {
            var result = await repository.UpdateNoticeAsync(dto, file, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update notice.");
            return Problem("Failed to update notice.", statusCode: 500);
        }
    }

    [HttpPatch("{id}/active-flag")]
    [EndpointSummary("Toggle a notice's active flag")]
    [EndpointDescription("Toggles the active flag for a specific notice.")]
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
            logger.LogError(ex, "Failed to update active flag for notice {noticeId}", id);
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