using HortiBts.Api.Repositories.Farmers;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Farmers
{
    [Route("api/farmer-verification")]
    [ApiController]
    public class FarmersVerificationController(IFarmersVerificationRepository farmersVerificationRepository, ILogger<FarmersVerificationController> logger) : ControllerBase
    {
        [HttpGet("get-farmers-list-by-village-from-ufp")]
        [EndpointSummary("Get list of farmers by village")]
        [EndpointDescription("Retrieves the list of farmers by village code.")]
        public async Task<IActionResult> GetFarmersListFromUFP([FromQuery] string VillageCode)
        {
            try
            {
                var result = await farmersVerificationRepository.GetFarmersListByVillageFromUFPAsync(VillageCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving farmers list for village code: {VillageCode}", VillageCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

    }
}


