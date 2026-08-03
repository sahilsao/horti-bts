using HortiBts.Shared.Dtos.Schemes;
using HortiBts.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Schemes;

[ApiController]
[Route("api/schemes")]
public class SchemeDocsController(ISchemeDocRepository repository, ILogger<SchemeDocsController> logger) : ControllerBase
{

    // GET api/schemes/136/documents?docType=SCHEME_NEW
    // docType is accepted for parity with the original contract but currently unused --
    // see the comment in SchemeDocRepository for why.
    [HttpGet("{schemeId:int}/documents")]
    public async Task<ActionResult<IEnumerable<SchemeDocDto>>> GetBySchemeId(int schemeId, [FromQuery] string? docType)
    {
        try
        {
            var docs = await repository.GetBySchemeIdAsync(schemeId);
            return Ok(docs);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load documents for schemeId={SchemeId}, docType={DocType}", schemeId, docType);
            return Problem("Failed to load scheme documents.", statusCode: 500);
        }
    }

}