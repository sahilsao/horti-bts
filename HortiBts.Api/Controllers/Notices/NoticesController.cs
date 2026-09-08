using HortiBts.Api.Repositories.Notices;
using HortiBts.Shared.Dtos.Notices;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers;

[ApiController]
[Route("api/notices")]
public class NoticesController(INoticeRepository repository, ILogger<NoticesController> logger) : ControllerBase
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
}