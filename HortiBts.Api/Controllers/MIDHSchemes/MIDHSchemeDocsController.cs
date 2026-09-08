using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;
using HortiBts.Api.Repositories.MIDHSchemes;
using HortiBts.Shared.Dtos.MIDHSchemes;

namespace HortiBts.Api.Controllers.MIDHSchemes;

[ApiController]
[Route("api/midh-schemes")]
public class MIDHSchemeDocsController(
    IMIDHSchemeDocRepository repository,
    ILogger<MIDHSchemeDocsController> logger) : ControllerBase
{
    // GET api/midh-schemes/{schemeId}/documents
    [HttpGet("{schemeId}/documents")]
    [EndpointSummary("Get midh scheme documents")]
    [EndpointDescription("Retrieves the midh documents associated with a specific scheme.")]
    public async Task<ActionResult<IEnumerable<MIDHSchemeDocDto>>> GetByMidhSchemeId(int schemeId, [FromQuery] string? docType)
    {
        try
        {
            var docs = await repository.GetByMidhSchemeIdAsync(schemeId);

            return Ok(docs);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to load midh documents for schemeId={SchemeId}, docType={DocType}",
                schemeId,
                docType);

            return Problem(
                "Failed to load midh scheme documents.",
                statusCode: 500);
        }
    }
}