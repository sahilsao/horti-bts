using HortiBts.Api.Repositories.ApplicationVerification.DDH.OldApplication;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/farmer-details")]
    [ApiController]
    public class FarmersDetailsController(IFarmersDetailsRepository farmersDetailsRepository, ILogger<FarmersDetailsController> logger) : ControllerBase
    {
        [HttpGet("get-basic-details-by-hfid")]
        [EndpointSummary("Get farmer basic details by hfid ID")]
        [EndpointDescription("Retrieves the basic details of a farmer using their Unique Farmer hfid ID.")]
        public async Task<IActionResult> GetFarmersBasicDetailsByHfId([FromQuery] int hfId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBasicDetailsByHfIdAsync(hfId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer basic details for Hf ID: {hfId}", hfId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-address-details-by-fdid")]
        [EndpointSummary("Get farmer address details by FD ID")]
        [EndpointDescription("Retrieves the address details of a farmer using their Unique Farmer FD ID.")]
        public async Task<IActionResult> GetFarmersAddressDetailsByFdId([FromQuery] int fdId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersAddressDetailsByFdIdAsync(fdId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer address details for FdId: {fdId}", fdId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-bank-details-by-fdid")]
        [EndpointSummary("Get farmer bank details by fd ID")]
        [EndpointDescription("Retrieves the bank details of a farmer using their Unique Farmer fd ID (fdId).")]
        public async Task<IActionResult> GetFarmersBankDetailsByFdId([FromQuery] int fdId)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersBankDetailsByFdIdAsync(fdId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer bank details for FdId: {fdId}", fdId);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-land-details-by-fdid")]
        [EndpointSummary("Get farmer land details by Fd ID")]
        [EndpointDescription("Retrieves the land details of a farmer using their Unique Farmer Fd ID (fdId).")]
        public async Task<IActionResult> GetFarmersLandDetailsByFdId([FromQuery] int fdId, [FromQuery] string villageType, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersLandDetailsByFdIdAsync(fdId, villageType, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer land details for fd Id: {fdId}, village Type: {villageType} and FinancialYear: {FinancialYear}", fdId, villageType, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-scheme-details-by-fdid")]
        [EndpointSummary("Get farmer scheme details by fd ID")]
        [EndpointDescription("Retrieves the scheme details of a farmer using their Unique Farmer fd ID (fdId).")]
        public async Task<IActionResult> GetFarmersSchemeDetailsByFdId([FromQuery] int fdId, [FromQuery] int sdId, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersSchemeDetailsByFdIdAsync(fdId, sdId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer scheme details for FdId: {fdId}, SdId: {sdId} and FinancialYear: {FinancialYear}", fdId, sdId, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-crop-details-by-fdid")]
        [EndpointSummary("Get farmer crop details by fd ID")]
        [EndpointDescription("Retrieves the crop details of a farmer using their Unique Farmer fd ID (fdId).")]
        public async Task<IActionResult> GetFarmersCropDetailsByFdId([FromQuery] int fdId, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersCropDetailsByFdIdAsync(fdId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer crop details for FdId: {fdId} and FinancialYear: {FinancialYear}", fdId, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-machinery-details-by-fdid")]
        [EndpointSummary("Get farmer machinery details by fd ID")]
        [EndpointDescription("Retrieves the machinery details of a farmer using their Unique Farmer fd ID (fdId).")]
        public async Task<IActionResult> GetFarmersMachineryDetailsByFdId([FromQuery] int fdId, [FromQuery] int financialYear)
        {
            try
            {
                var result = await farmersDetailsRepository.GetFarmersMachineryDetailsByFdIdAsync(fdId, financialYear);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmer machinery details for FdId: {fdId} and FinancialYear: {FinancialYear}", fdId, financialYear);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
