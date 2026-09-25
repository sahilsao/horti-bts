using HortiBts.Api.Repositories.Reports.PreviousFY.BeneficiaryStatus;
using HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.PreviousFY.BeneficiaryStatus
{
    [Route("api/reports/beneficiary/previous")]
    [ApiController]
    public class FarmerBeneficiaryStatusController(IFarmerBeneficiaryStatusRepository repository,
        ILogger<DistWiseFarmerBeneficiaryStatusDto> logger,
        ILogger<BlockWiseFarmerBeneficiaryStatusDto> logger2,
        ILogger<VillageWiseFarmerBeneficiaryStatusDto> logger3,
        ILogger<FarmerWiseFarmerBeneficiaryStatusDto> logger4) : ControllerBase
    {
        [HttpGet("distwise-farmer-status")]
        [EndpointSummary("Get beneficiary status report of district-wise farmer")]
        [EndpointDescription("Retrieves the beneficiary status report of district-wise farmer registration.")]
        public async Task<IActionResult> GetBeneficiaryStatusRptOfDistWiseFarmerAsync()
        {
            try
            {
                var result = await repository.GetRptOfDistWiseFarmerBeneficiaryStatusAsync();
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load beneficiary status report of district-wise farmer registration.");
                return Problem(detail: "Failed to load beneficiary status report of district-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("blockwise-farmer-status")]
        [EndpointSummary("Get beneficiary status report of block-wise farmer")]
        [EndpointDescription("Retrieves the beneficiary status report of block-wise farmer registration.")]
        public async Task<IActionResult> GetBeneficiaryStatusRptOfBlockWiseFarmerAsync([FromQuery] int districtCode)
        {
            try
            {
                var result = await repository.GetRptOfBlockWiseFarmerBeneficiaryStatusAsync(districtCode);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load beneficiary status report of district-wise farmer registration.");
                return Problem(detail: "Failed to load beneficiary status report of district-wise farmer registration.", statusCode: 500);
            }
        }
        [HttpGet("villagewise-farmer-status")]
        [EndpointSummary("Get beneficiary status report of village-wise farmer")]
        [EndpointDescription("Retrieves the beneficiary status report of village-wise farmer registration.")]
        public async Task<IActionResult> GetBeneficiaryStatusRptOfVillageWiseFarmerAsync([FromQuery] int subDistrictCode)
        {
            try
            {
                var result = await repository.GetRptOfVillageWiseFarmerBeneficiaryStatusAsync(subDistrictCode);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger3.LogError(ex, "Failed to load beneficiary status report of village-wise farmer registration.");
                return Problem(detail: "Failed to load beneficiary status report of village-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("farmerwise-farmer-status")]
        [EndpointSummary("Get farmer-wise farmer beneficiary status report")]
        [EndpointDescription("Retrieves the beneficiary status report of farmer-wise farmer registration. Optionally filters by status code.")]
        public async Task<IActionResult> GetBeneficiaryStatusRptOfFarmerWiseFarmerAsync([FromQuery] int villageCode, [FromQuery] int statusCode)
        {
            try
            {
                var result = await repository.GetRptOfFarmerWiseFarmerBeneficiaryStatusAsync(villageCode, statusCode);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger4.LogError(ex, "Failed to load beneficiary status report of farmer-wise farmer registration.");
                return Problem(detail: "Failed to load beneficiary status report of farmer-wise farmer registration.", statusCode: 500);
            }
        }
    }
}
