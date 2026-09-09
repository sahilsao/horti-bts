using HortiBts.Api.Controllers.Components;
using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Benefits;
using HortiBts.Shared.Dtos.Benefits;
using HortiBts.Shared.Dtos.Flag;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Benefits
{
    [Route("api/benefits")]
    [ApiController]
    public class BenefitsController(IBenefitsRepository benefitsRepository, IHttpContextAccessor httpContextAccessor, ILogger<ComponentsController> logger) : ControllerBase
    {
        [HttpGet("benefits-types")]
        [EndpointSummary("Get all benefits types")]
        [EndpointDescription("Retrieves the list of all benefits types available in the system.")]
        public async Task<IActionResult> GetBenefits()
        {
            try
            {
                var result = await benefitsRepository.GetBenefitsTypeAsync();

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load benefits types.");
                return Problem(detail: "Failed to load benefits types.", statusCode: 500);
            }
        }

        [HttpGet("benefits-list")]
        [EndpointSummary("Get all benefits list")]
        [EndpointDescription("Retrieves the list of all benefits available in the system.")]
        public async Task<IActionResult> GetBenefitsList()
        {
            try
            {
                var result = await benefitsRepository.GetBenefitsListAsync();

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load benefits list.");
                return Problem(detail: "Failed to load benefits list.", statusCode: 500);
            }
        }

        [HttpPost("save-benefit-data")]
        [EndpointSummary("Save a new benefit")]
        [EndpointDescription("Inserts a new benefit record linked to a benefit type.")]
        public async Task<IActionResult> SaveBenefitData([FromBody] AddBenefitDto dto)
        {
            try
            {
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var insertedBy = User.Identity?.Name ?? "0"; // adjust to however you extract the logged-in user id/claim

                var result = await benefitsRepository.SaveBenefitAsync(dto, insertedBy, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result); // returns the new benefit_id, matching raw-value client expectation
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save benefit data.");
                return Problem(detail: "Failed to save benefit data.", statusCode: 500);
            }
        }

        [HttpPatch("{id}/flag")]
        [EndpointSummary("Toggle a benefit's active status")]
        public async Task<IActionResult> UpdateFlag(int id, [FromBody] UpdateFlagDto dto)
        {
            try
            {
                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await benefitsRepository.UpdateFlagAsync(id, dto.Flag, userId, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update benefit flag.");
                return Problem(detail: "Failed to update benefit flag.", statusCode: 500);
            }
        }
    }
}
