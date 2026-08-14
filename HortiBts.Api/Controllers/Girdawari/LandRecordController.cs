using HortiBts.Api.Repositories.Girdawari;
using HortiBts.Shared.Common;
using HortiBts.Shared.Models.Girdawari;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LandRecordController(ICropDetailRepository cropDetailRepository) : ControllerBase
{

    // POST api/landrecord/get-girdawari-details
    [HttpPost("get-girdawari-details")]
    [EndpointSummary("Get Girdawari crop details")]
    [EndpointDescription(
    "Retrieves crop and Girdawari details from the land-record service " +
    "for the supplied village census codes and Khasra numbers.")]
    [ProducesResponseType(typeof(Result<List<CropDetailRecord[]>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<CropDetailRecord[]>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<List<CropDetailRecord[]>>), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetGirdawariDetails(
        [FromBody] List<SearchParam>? searchParams,
        CancellationToken cancellationToken)
    {
        if (searchParams is null || searchParams.Count == 0)
        {
            // Original returned a bare "Empty" string; a structured 400 is
            // more useful to API consumers while keeping the same intent.
            return BadRequest(Result<List<CropDetailRecord[]>>.Failure("Empty"));
        }

        var result = await cropDetailRepository.GetCropDetailsAsync(searchParams, cancellationToken);

        // Logging of the underlying exception already happens inside the
        // repository, right where the stack trace is available.
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(StatusCodes.Status502BadGateway, result);
    }
}



