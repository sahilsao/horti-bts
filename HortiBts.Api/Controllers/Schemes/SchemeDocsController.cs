using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;
using HortiBts.Api.Repositories.Schemes;

namespace HortiBts.Api.Controllers.Schemes;

[ApiController]
[Route("api/schemes")]
public class SchemeDocsController(
    ISchemeDocRepository repository,
    ILogger<SchemeDocsController> logger) : ControllerBase
{
    // GET api/schemes/136/documents?docType=SCHEME_NEW
    // docType is accepted for parity with the original contract but currently unused.
    [HttpGet("{schemeId}/documents")]
    [EndpointSummary("Get scheme documents")]
    [EndpointDescription("Retrieves the documents associated with a specific scheme. An optional document type can be provided to maintain compatibility with the existing API contract.")]
    public async Task<ActionResult<IEnumerable<SchemeDocDto>>> GetBySchemeId(
        int schemeId,
        [FromQuery] string? docType)
    {
        try
        {
            var docs = await repository.GetBySchemeIdAsync(schemeId);

            return Ok(docs);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to load documents for schemeId={SchemeId}, docType={DocType}",
                schemeId,
                docType);

            return Problem(
                "Failed to load scheme documents.",
                statusCode: 500);
        }
    }
}