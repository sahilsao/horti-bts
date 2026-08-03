using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Schemes;

[ApiController]
[Route("api/schemes")]
public class SchemesController(ISchemeRepository repository, ILogger<SchemesController> logger) : ControllerBase
{

    // GET api/schemes/2  (2 = centrally sponsored)
    // GET api/schemes/1  (1 = state sponsored)
    [HttpGet("{stId:int}")]
    public async Task<ActionResult<IEnumerable<SchemeDto>>> GetByType(int stId)
    {
        try
        {
            var schemes = await repository.GetByTypeAsync(stId);
            return Ok(schemes);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load schemes for stId={StId}", stId);
            return Problem("Failed to load schemes.", statusCode: 500);
        }
    }
}