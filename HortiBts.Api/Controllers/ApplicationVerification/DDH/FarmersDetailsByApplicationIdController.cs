using HortiBts.Api.Repositories.ApplicationVerification.DDH;
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
        public async Task<IActionResult> GetFarmersBasicByApplicationIdDetails([FromQuery] int ApplicationId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBasicDetailsByApplicationIdAsync(ApplicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer basic details for Application ID: {ApplicationId}", ApplicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-address-details")]
        [EndpointSummary("Get farmer address details by application ID")]
        [EndpointDescription("Retrieves the address details of a farmer using their Unique Farmer Application ID.")]
        public async Task<IActionResult> GetFarmersAddressByApplicationIdDetails([FromQuery] int ApplicationId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersAddressDetailsByApplicationIdAsync(ApplicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer address details for ApplicationId: {ApplicationId}", ApplicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-bank-details")]
        [EndpointSummary("Get farmer bank details by application ID")]
        [EndpointDescription("Retrieves the bank details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersBankByApplicationIdDetails([FromQuery] int ApplicationId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBankDetailsByApplicationIdAsync(ApplicationId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer bank details for ApplicationId: {ApplicationId}", ApplicationId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-land-details")]
        [EndpointSummary("Get farmer land details by application ID")]
        [EndpointDescription("Retrieves the land details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersLandByApplicationIdDetails([FromQuery] int ApplicationId, int FinancialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersLandDetailsByApplicationIdAsync(ApplicationId, FinancialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer land details for ApplicationId: {ApplicationId} and FinancialYear: {FinancialYear}", ApplicationId, FinancialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-scheme-details")]
        [EndpointSummary("Get farmer scheme details by application ID")]
        [EndpointDescription("Retrieves the scheme details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersSchemeByApplicationIdDetails([FromQuery] int ApplicationId, int FinancialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersSchemeDetailsByApplicationIdAsync(ApplicationId, FinancialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer scheme details for ApplicationId: {ApplicationId} and FinancialYear: {FinancialYear}", ApplicationId, FinancialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-crop-details")]
        [EndpointSummary("Get farmer crop details by application ID")]
        [EndpointDescription("Retrieves the crop details of a farmer using their Unique Farmer Application ID (ApplicationId).")]
        public async Task<IActionResult> GetFarmersCropByApplicationIdDetails([FromQuery] int ApplicationId, int FinancialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersCropDetailsByApplicationIdAsync(ApplicationId, FinancialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer crop details for ApplicationId: {ApplicationId} and FinancialYear: {FinancialYear}", ApplicationId, FinancialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
