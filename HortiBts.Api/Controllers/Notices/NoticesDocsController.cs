using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;
using HortiBts.Api.Repositories.Schemes;
using HortiBts.Api.Repositories.Notices;
using HortiBts.Shared.Dtos.Notices;

namespace HortiBts.Api.Controllers.Notices;

[ApiController]
[Route("api/notices")]
public class NoticeDocsController(
    INoticeDocRepository repository,
    ILogger<NoticeDocsController> logger) : ControllerBase
{
    [HttpGet("{noticeId}/documents")]
    [EndpointSummary("Get notice documents")]
    [EndpointDescription("Retrieves the documents associated with a specific notice. An optional document type can be provided to maintain compatibility with the existing API contract.")]
    public async Task<ActionResult<IEnumerable<NoticeDocDto>>> GetByNoticeId(int noticeId, [FromQuery] string? docType)
    {
        try
        {
            var docs = await repository.GetByNoticeIdAsync(noticeId);
            return Ok(docs);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load documents for noticeId={noticeId}, docType={DocType}", noticeId, docType);
            return Problem("Failed to load notice documents.", statusCode: 500);
        }
    }
}