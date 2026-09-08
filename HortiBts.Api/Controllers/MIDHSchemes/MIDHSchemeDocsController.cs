using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;
using HortiBts.Api.Repositories.MIDHSchemes;

namespace HortiBts.Api.Controllers.MIDHSchemes;

[ApiController]
[Route("api/midh-schemes")]
public class MIDHSchemeDocsController(
    IMIDHSchemeDocRepository repository,
    ILogger<MIDHSchemeDocsController> logger) : ControllerBase
{
    // GET api/midh-schemes/{midhschemeId}/documents
    [HttpGet("{midhschemeId}/documents")]
    [EndpointSummary("Get midh scheme documents")]
    [EndpointDescription("Retrieves the midh documents associated with a specific scheme.")]
    public async Task<ActionResult<IEnumerable<SchemeDocDto>>> GetByMidhSchemeId(int midhschemeId, [FromQuery] string? docType)
    {
        try
        {
            var docs = await repository.GetByMidhSchemeIdAsync(midhschemeId);

            return Ok(docs);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to load midh documents for schemeId={SchemeId}, docType={DocType}",
                midhschemeId,
                docType);

            return Problem(
                "Failed to load midh scheme documents.",
                statusCode: 500);
        }
    }
}