using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Benefits;
using HortiBts.Shared.Dtos.Benefits;
using HortiBts.Shared.Dtos.Flag;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Benefits
{
    [Route("api/benefits")]
    [ApiController]
    public class BenefitsController(IBenefitsRepository benefitsRepository, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpGet("benefits-types")]
        [EndpointSummary("Get all benefits types")]
        [EndpointDescription("Retrieves the list of all benefits types available in the system.")]
        public async Task<IActionResult> GetBenefits()
        {
            var result = await benefitsRepository.GetBenefitsTypeAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result.Data);
        }

        [HttpGet("benefits-list")]
        [EndpointSummary("Get all benefits list")]
        [EndpointDescription("Retrieves the list of all benefits available in the system.")]
        public async Task<IActionResult> GetBenefitsList()
        {
            var result = await benefitsRepository.GetBenefitsListAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result.Data);
        }

        [HttpPost("save-benefit-data")]
        [EndpointSummary("Save a new benefit")]
        [EndpointDescription("Inserts a new benefit record linked to a benefit type.")]
        public async Task<IActionResult> SaveBenefitData([FromBody] AddBenefitDto dto)
        {
            var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

            var insertedBy = User.Identity?.Name ?? "system"; // adjust to however you extract the logged-in user id/claim

            var result = await benefitsRepository.SaveBenefitAsync(dto, insertedBy, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result.Data); // returns the new benefit_id, matching raw-value client expectation
        }

        [HttpPatch("{id}/flag")]
        [EndpointSummary("Toggle a benefit's active status")]
        public async Task<IActionResult> UpdateFlag(int id, [FromBody] UpdateFlagDto dto)
        {
            var userId = User.Identity?.Name ?? "system";
            var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

            var result = await benefitsRepository.UpdateFlagAsync(id, dto.Flag, userId, clientIp);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result.Data);
        }
    }
}
