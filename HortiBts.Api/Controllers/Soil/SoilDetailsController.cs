using HortiBts.Api.Services;
using HortiBts.Api.Services.Soil;
using HortiBts.Shared.Common;
using HortiBts.Shared.Models.Soil;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Soil
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SoilDetailsController(ISoilDetailsService service) : ControllerBase
    {
        // GET api/soil?villageCode=433493&khasraNo=574/1
        [HttpGet]
        [ProducesResponseType(typeof(Result<List<SoilDetailsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<SoilDetailsDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<List<SoilDetailsDto>>), StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> Get(
            [FromQuery] string villageCode,
            [FromQuery] string khasraNo,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(villageCode) || string.IsNullOrWhiteSpace(khasraNo))
                return BadRequest(Result<List<SoilDetailsDto>>.Failure("villageCode and khasraNo are required."));

            var result = await service.GetAsync(villageCode, khasraNo, ct);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode(StatusCodes.Status502BadGateway, result);
        }
    }
}
