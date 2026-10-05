using HortiBts.Api.Repositories.HPMIS;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Farmers
{
    [Route("api/reports/hpmis")]
    [Tags("Farmer Details (HPMIS)")]
    [ApiController]
    public class FarmersDetailsHPMISController(IFarmersDetailsHPMISRepository farmersDetailsHPMISRepository, ILogger<FarmersDetailsHPMISController> logger) : ControllerBase
    {
        [HttpGet("get-basic-details")]
        [EndpointSummary("Get farmer basic details")]
        [EndpointDescription("Retrieves the basic details of a farmer using their Application ID.")]
        public async Task<IActionResult> GetFarmersBasicDetails([FromQuery] string applicationId)
        {
            try
            {
                var result = await farmersDetailsHPMISRepository.GetFarmersBasicDetailsAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer basic details for Application ID: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-application-details")]
        [EndpointSummary("Get farmer application details")]
        [EndpointDescription("Retrieves the application details of a farmer using their Unique Application ID.")]
        public async Task<IActionResult> GetFarmersApplicationDetails([FromQuery] string applicationId)
        {
            try
            {
                var result = await farmersDetailsHPMISRepository.GetFarmersApplicationDetailsAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer application details for Application ID: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }


        [HttpGet("get-land-details")]
        [EndpointSummary("Get farmer land details")]
        [EndpointDescription("Retrieves the land details of a farmer using their Unique Application ID.")]
        public async Task<IActionResult> GetFarmersLandDetails([FromQuery] string applicationId)
        {
            try
            {
                var result = await farmersDetailsHPMISRepository.GetFarmersLandDetailsAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer land details for Application ID: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-crop-details")]
        [EndpointSummary("Get farmer crop details")]
        [EndpointDescription("Retrieves the crop details of a farmer using their Unique Application ID.")]
        public async Task<IActionResult> GetFarmersCropDetails([FromQuery] string applicationId)
        {
            try
            {
                var result = await farmersDetailsHPMISRepository.GetFarmersCropDetailsAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer crop details for Application ID: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-market-linkage-details")]
        [EndpointSummary("Get farmer market linkage details")]
        [EndpointDescription("Retrieves the market linkage details of a farmer using their Unique Application ID.")]
        public async Task<IActionResult> GetFarmersMarketLinkageDetails([FromQuery] string applicationId)
        {
            try
            {
                var result = await farmersDetailsHPMISRepository.GetFarmersMarketLinkageDetailsAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer market linkage details for Application ID: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-scheme-details")]
        [EndpointSummary("Get farmer scheme details")]
        [EndpointDescription("Retrieves the scheme details of a farmer using their Unique Application ID.")]
        public async Task<IActionResult> GetFarmersSchemeDetails([FromQuery] string applicationId)
        {
            try
            {
                var result = await farmersDetailsHPMISRepository.GetFarmersSchemeDetailsAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer scheme details for Application ID: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }


    }
}
