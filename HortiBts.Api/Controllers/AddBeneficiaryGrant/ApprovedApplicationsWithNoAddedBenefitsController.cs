using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.AddBeneficiaryGrant;
using HortiBts.Shared.Dtos.AddBeneficiaryGrant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.AddBeneficiaryGrant
{
    [Route("api/approved-applications")]
    [ApiController]
    public class ApprovedApplicationsWithNoAddedBenefitsController(IApprovedApplicationsWithNoAddedBenefitsRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<ApprovedApplicationsWithNoAddedBenefitsController> logger) : ControllerBase
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

        [HttpPost("save-application-scheme-benefit-data")]
        [EndpointSummary("Save approved application benefit and its scheme benefit rows")]
        [EndpointDescription("Inserts the approved benefit and its scheme benefit rows in one transaction.")]
        public async Task<IActionResult> SaveApplicationSchemeBenefitData([FromBody] SaveApplicationSchemeBenefitDataDto dto)
        {
            try
            {
                if (dto.ApplicationId <= 0 || dto.BenefitIds.Count == 0)
                    return Problem(detail: "Invalid data.", statusCode: 400);

                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
                var createdBy = User.Identity?.Name ?? "0"; // same as your BenefitsController

                var result = await repository.SaveApplicationSchemeBenefitDataAsync(dto, createdBy, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result); // Result<int> = new ab_id
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save approved application benefit.");
                return Problem(detail: "Failed to save approved application benefit.", statusCode: 500);
            }
        }
    }
}
