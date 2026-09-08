using HortiBts.Api.Repositories.Farmers;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Farmers
{
    [Route("api/farmer-verification")]
    [ApiController]
    public class FarmersVerificationController(IFarmersVerificationRepository farmersVerificationRepository) : ControllerBase
    {
        [HttpGet("get-farmers-list-by-village-from-ufp")]
        [EndpointSummary("Get list of farmers by village")]
        [EndpointDescription("Retrieves the list of farmers by village code.")]
        public async Task<IActionResult> GetFarmersListFromUFP([FromQuery] string VillageCode)
        {
            var result = await farmersVerificationRepository.GetFarmersListByVillageFromUFPAsync(VillageCode);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }

    }
}


