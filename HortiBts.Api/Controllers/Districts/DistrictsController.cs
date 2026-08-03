using HortiBts.Api.Repository.Districts;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Districts
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController(IDistrictsRepository districtRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetDistricts()
        {
            var result = await districtRepository.GetDistrictsAsync();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result.Value);
        }
    }
}
