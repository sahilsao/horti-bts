using System;
using HortiBts.Api.Repositories.ApplicationVerification.DDH;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/district")]
    [ApiController]
    public class SchemeWiseOldBeneficiaryApplicationController(ISchemeWiseOldBeneficiaryApplicationRepository repository, ILogger<SchemeWiseOldBeneficiaryApplicationListDto> logger) : ControllerBase
    {


        [HttpGet("scheme-wise-old-beneficiaries-applications")]
        [EndpointSummary("Get beneficiary list of old applications")]
        [EndpointDescription("Retrieves the list of old beneficiaries applications for a specified district, scheme, and financial year.")]
        public async Task<IActionResult> GetSchemeWiseDistrictApplicationsAsync([FromQuery] int schemeId, [FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetSchemeWiseOldBeneficiaryApplicationsAsync(schemeId, districtCode, financialYear);
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
