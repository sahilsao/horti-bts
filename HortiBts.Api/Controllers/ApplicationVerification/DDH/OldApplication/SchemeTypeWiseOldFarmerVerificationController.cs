
using HortiBts.Api.Repositories.ApplicationVerification.DDH.OldApplication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{

    [Route("api/district")]
    [ApiController]
    public class SchemeTypeWiseOldFarmerVerificationController(
   ISchemeTypeWiseOldFarmerVerificationRepository repository,
   ILogger<SchemeTypeWiseOldFarmerVerificationController> logger) : ControllerBase
    {
        [HttpGet("scheme-type-wise-old-applications")]
        [EndpointSummary("Get scheme type wise district old applications")]
        [EndpointDescription("Retrieves the list of old applications for a filtered by district, sub-district, officer or village, depending on the search flag.")]
        public async Task<IActionResult> GetSchemeTypeWiseOldDistrictApplicationsAsync(
            [FromQuery] int districtCode,
            [FromQuery] int subDistrictCode,
            [FromQuery] int villageCode,
            [FromQuery] int officerCode,
            [FromQuery] int schemeTypeId,
            [FromQuery] int financialYear)
        {

            try
            {
                var result = await repository.GetSchemeTypeWiseOlFarmerVerificationAsync(districtCode, subDistrictCode, villageCode, officerCode, schemeTypeId, financialYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load applications list");
                return Problem("Failed to load applications list details.", statusCode: 500);
            }
        }
    }
}

