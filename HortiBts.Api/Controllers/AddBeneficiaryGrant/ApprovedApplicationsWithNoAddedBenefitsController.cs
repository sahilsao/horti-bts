using HortiBts.Api.Repositories.AddBeneficiaryGrant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.AddBeneficiaryGrant
{
    [Route("api/approved-applications")]
    [ApiController]
    public class ApprovedApplicationsWithNoAddedBenefitsController(IApprovedApplicationsWithNoAddedBenefitsRepository repository, ILogger<ApprovedApplicationsWithNoAddedBenefitsController> logger) : ControllerBase
    {
        [HttpGet("with-no-added-benefits-by-district")]
        [EndpointSummary("Get all approved applications with no added benefits by district code")]
        [EndpointDescription("Retrieves the list of all approved applications with no added benefits by district code available in the system.")]
        public async Task<IActionResult> GetApprovedApplicationsWithNoAddedBenefitsByDistrictCode(int districtCode, int financialYear)
        {
            try
            {
                var result = await repository.GetApprovedApplicationsWithNoAddedBenefitsByDistrictCodeAsync(districtCode, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load approved applications with no added benefits by district code.");
                return Problem(detail: "Failed to load approved applications with no added benefits by district code.", statusCode: 500);
            }
        }

        [HttpGet("with-no-added-benefits-by-application-id")]
        [EndpointSummary("Get all approved applications with no added benefits by application id")]
        [EndpointDescription("Retrieves the list of all approved applications with no added benefits by application id available in the system.")]
        public async Task<IActionResult> GetApprovedApplicationsWithNoAddedBenefitsByApplicationId(int applicationId, int financialYear)
        {
            try
            {
                var result = await repository.GetApprovedApplicationsWithNoAddedBenefitsByApplicationIDAsync(applicationId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load approved applications with no added benefits by application id.");
                return Problem(detail: "Failed to load approved applications with no added benefits by application id.", statusCode: 500);
            }
        }
    }
}
