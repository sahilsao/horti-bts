using HortiBts.Api.Repositories.Farmers;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Farmers
{
    [Route("api/farmer-details")]
    [ApiController]
    public class FarmersDetailsController(IFarmersDetailsRepository farmersDetailsRepository, ILogger<FarmersDetailsController> logger) : ControllerBase
    {
        [HttpGet("get-basic-details")]
        [EndpointSummary("Get farmer basic details")]
        [EndpointDescription("Retrieves the basic details of a farmer using their Unique Farmer ID (UFID).")]
        public async Task<IActionResult> GetFarmersBasicDetails([FromQuery] string UFID)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBasicDetailsAsync(UFID);

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
        public async Task<IActionResult> GetFarmersAddressDetails([FromQuery] string UFID)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersAddressDetailsAsync(UFID);

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
        public async Task<IActionResult> GetFarmersBankDetails([FromQuery] string UFID)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBankDetailsAsync(UFID);

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
        public async Task<IActionResult> GetFarmersLandDetails([FromQuery] string UFID, string FinYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersLandDetailsAsync(UFID, FinYear);

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
        public async Task<IActionResult> GetFarmersSchemeDetails([FromQuery] string UFID, string FinYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersSchemeDetailsAsync(UFID, FinYear);

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
                var result = await farmersDetailsRepository.GetFarmersCropDetailsAsync(UFID, FinYear);

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
