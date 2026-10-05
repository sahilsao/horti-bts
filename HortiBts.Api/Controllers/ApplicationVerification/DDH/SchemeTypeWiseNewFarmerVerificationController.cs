using System;
using HortiBts.Api.Repositories.ApplicationVerification.DDH;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/district")]
    [ApiController]
    public class SchemeTypeWiseNewFarmerVerificationController(ISchemeTypeWiseNewFarmerVerificationRepository repository, ILogger<SchemeTypeWiseNewApplicationsListDto> logger) : ControllerBase
    {
        [HttpGet("scheme-type-wise-new-applications")]
        [EndpointSummary("Get scheme type wise new district applications")]
        [EndpointDescription("Retrieves the list of new applications for a specified district, scheme type, and financial year.")]
        public async Task<IActionResult> GetSchemeTypeWiseNewDistrictApplicationsAsync([FromQuery] int schemeTypeId, [FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetSchemeTypeWiseNewDistrictApplicationsAsync(schemeTypeId, districtCode, financialYear);
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
