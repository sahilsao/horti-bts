using System;
using HortiBts.Api.Repositories.ApplicationVerification.DDH.NewApplication;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH.NewApplication;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/district")]
    [ApiController]
    public class SchemeWiseNewBeneficiaryApplicationController(ISchemeWiseNewBeneficiaryApplicationRepository repository, ILogger<SchemeWiseNewBeneficiaryApplicationController> logger) : ControllerBase
    {


        [HttpGet("scheme-wise-new-beneficiaries-applications")]
        [EndpointSummary("Get beneficiary list of new applications")]
        [EndpointDescription("Retrieves the list of new beneficiaries applications for a specified district, scheme, and financial year.")]
        public async Task<IActionResult> GetSchemeWiseNewBeneficiaryApplicationsAsync([FromQuery] int schemeId, [FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetSchemeWiseNewBeneficiaryApplicationsAsync(schemeId, districtCode, financialYear);
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
