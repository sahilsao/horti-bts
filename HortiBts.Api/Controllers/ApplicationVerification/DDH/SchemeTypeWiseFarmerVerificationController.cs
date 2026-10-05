using System;
using HortiBts.Api.Repositories.ApplicationVerification.DDH;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/district")]
    [ApiController]
    public class SchemeTypeWiseFarmerVerificationController(ISchemeTypeWiseFarmerVerificationRepository repository, ILogger<SchemeTypeWiseApplicationsListDto> logger) : ControllerBase
    {
        [HttpGet("scheme-type-wise-applications")]
        [EndpointSummary("Get scheme type wise district applications")]
        [EndpointDescription("Retrieves the list of applications for a specified district, scheme type, and financial year.")]
        public async Task<IActionResult> GetSchemeTypeWiseDistrictApplicationsAsync([FromQuery] int schemeTypeId, [FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetSchemeTypeWiseDistrictApplicationsAsync(schemeTypeId, districtCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load scheme-type wise district applications.");
                return Problem(detail: "Failed to load scheme-typee wise district applications.", statusCode: 500);
            }
        }
    }
}
