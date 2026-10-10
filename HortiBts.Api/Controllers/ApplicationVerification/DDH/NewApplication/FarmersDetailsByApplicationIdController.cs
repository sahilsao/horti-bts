using HortiBts.Api.Repositories.ApplicationVerification.DDH.NewApplication;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/farmer-details-by-application-id")]
    [ApiController]
    public class FarmersDetailsByApplicationIdController(IFarmersDetailsByApplicationIdRepository farmersDetailsRepository, ILogger<FarmersDetailsByApplicationIdController> logger) : ControllerBase
    {
        [HttpGet("get-basic-details")]
        [EndpointSummary("Get farmer basic details by application ID")]
        [EndpointDescription("Retrieves the basic details of a farmer using their Unique Farmer Application ID.")]
        public async Task<IActionResult> GetFarmersBasicDetailsByApplicationId([FromQuery] int applicationId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBasicDetailsByApplicationIdAsync(applicationId);

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

        [HttpGet("get-address-details")]
        [EndpointSummary("Get farmer address details by application ID")]
        [EndpointDescription("Retrieves the address details of a farmer using their Unique Farmer Application ID.")]
        public async Task<IActionResult> GetFarmersAddressDetailsByApplicationId([FromQuery] int applicationId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersAddressDetailsByApplicationIdAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer address details for ApplicationId: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-bank-details")]
        [EndpointSummary("Get farmer bank details by application ID")]
        [EndpointDescription("Retrieves the bank details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersBankDetailsByApplicationId([FromQuery] int applicationId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBankDetailsByApplicationIdAsync(applicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer bank details for ApplicationId: {ApplicationId}", applicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-land-details")]
        [EndpointSummary("Get farmer land details by application ID")]
        [EndpointDescription("Retrieves the land details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersLandDetailsByApplicationId([FromQuery] int applicationId, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersLandDetailsByApplicationIdAsync(applicationId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer land details for ApplicationId: {ApplicationId} and FinancialYear: {FinancialYear}", applicationId, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-scheme-details")]
        [EndpointSummary("Get farmer scheme details by application ID")]
        [EndpointDescription("Retrieves the scheme details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersSchemeDetailsByApplicationId([FromQuery] int applicationId, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersSchemeDetailsByApplicationIdAsync(applicationId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer scheme details for ApplicationId: {ApplicationId} and FinancialYear: {FinancialYear}", applicationId, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-crop-details")]
        [EndpointSummary("Get farmer crop details by application ID")]
        [EndpointDescription("Retrieves the crop details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersCropDetailsByApplicationId([FromQuery] int applicationId, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersCropDetailsByApplicationIdAsync(applicationId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer crop details for ApplicationId: {ApplicationId} and FinancialYear: {FinancialYear}", applicationId, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
