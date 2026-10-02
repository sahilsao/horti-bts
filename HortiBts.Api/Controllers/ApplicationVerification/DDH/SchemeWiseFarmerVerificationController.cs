using System;
using HortiBts.Api.Repositories.ApplicationVerification.DDH;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/district")]
    [ApiController]
    public class SchemeWiseFarmerVerificationController(ISchemeWiseFarmerVerificationRepository repository, ILogger<SchemeWiseApplicationsListDto> logger) : ControllerBase
    {
        [HttpGet("scheme-wise-applications")]
        [EndpointSummary("Get scheme-wise district applications")]
        [EndpointDescription("Retrieves the list of applications for a specified district, scheme, and financial year.")]
        public async Task<IActionResult> GetSchemeWiseDistrictApplicationsAsync([FromQuery] int financialYear, [FromQuery] int districtCode, [FromQuery] int schemeTypeId)
        {
            try
            {
                var result = await repository.GetSchemeWiseDistrictApplicationsAsync(financialYear, districtCode, schemeTypeId);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load scheme-wise district applications.");
                return Problem(detail: "Failed to load scheme-wise district applications.", statusCode: 500);
            }
        }
    }
}
