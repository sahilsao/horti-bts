using HortiBts.Api.Repositories.Farmers;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Farmers
{
    [Route("api/farmer-details-by-ufid")]
    [ApiController]
    public class FarmersDetailsByUFIDController(IFarmersDetailsByUFIDRepository farmersDetailsRepository, ILogger<FarmersDetailsByUFIDController> logger) : ControllerBase
    {
        [HttpGet("get-basic-details")]
        [EndpointSummary("Get farmer basic details")]
        [EndpointDescription("Retrieves the basic details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersBasicByUFIDDetails([FromQuery] string UFID)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBasicDetailsByUFIDAsync(UFID);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer basic details for UFID: {UFID}", UFID);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-address-details")]
        [EndpointSummary("Get farmer address details")]
        [EndpointDescription("Retrieves the address details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersAddressByUFIDDetails([FromQuery] string UFID)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersAddressDetailsByUFIDAsync(UFID);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer address details for UFID: {UFID}", UFID);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-bank-details")]
        [EndpointSummary("Get farmer bank details")]
        [EndpointDescription("Retrieves the bank details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersBankByUFIDDetails([FromQuery] string UFID)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBankDetailsByUFIDAsync(UFID);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer bank details for UFID: {UFID}", UFID);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-land-details")]
        [EndpointSummary("Get farmer land details")]
        [EndpointDescription("Retrieves the land details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersLandByUFIDDetails([FromQuery] string UFID, string FinYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersLandDetailsByUFIDAsync(UFID, FinYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer land details for UFID: {UFID} and FinYear: {FinYear}", UFID, FinYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-scheme-details")]
        [EndpointSummary("Get farmer scheme details")]
        [EndpointDescription("Retrieves the scheme details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersSchemeByUFIDDetails([FromQuery] string UFID, string FinYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersSchemeDetailsByUFIDAsync(UFID, FinYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer scheme details for UFID: {UFID} and FinYear: {FinYear}", UFID, FinYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-crop-details")]
        [EndpointSummary("Get farmer crop details")]
        [EndpointDescription("Retrieves the crop details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersCropDetails([FromQuery] string UFID, string FinYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersCropDetailsByUFIDAsync(UFID, FinYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer crop details for UFID: {UFID} and FinYear: {FinYear}", UFID, FinYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
