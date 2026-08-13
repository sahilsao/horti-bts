using HortiBts.Api.Repositories.Schemes;
using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Schemes;

[ApiController]
[Route("api/schemes")]
public class SchemesController(
    ISchemeRepository repository,
    ILogger<SchemesController> logger) : ControllerBase
{
    // GET api/schemes/2  (2 = centrally sponsored)
    // GET api/schemes/1  (1 = state sponsored)
    [HttpGet("{stId:int}")]
    [EndpointSummary("Get schemes by type")]
    [EndpointDescription("Retrieves the list of schemes based on the specified scheme type. Use 1 for state-sponsored schemes and 2 for centrally sponsored schemes.")]
    public async Task<ActionResult<IEnumerable<SchemeDto>>> GetByType(
        [FromRoute] int stId)
    {
        try
        {
            var schemes = await repository.GetByTypeAsync(stId);

            return Ok(schemes);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to load schemes for stId={StId}",
                stId);

            return Problem(
                "Failed to load schemes.",
                statusCode: 500);
        }
    }
}