using HortiBts.Api.Repositories.Reports.HPMIS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.HPMIS
{

    [Route("api/reports")]
    [Tags("FarmerDetails (HPMIS)")]
    [ApiController]
    public class FarmerDetailsController(
    IFarmerDetailsRepository repository,
    ILogger<FarmerDetailsController> logger) : ControllerBase
    {
        // GET api/reports/farmer-list?searchFlag=DIST&financialYear=2025-26&id=100
        [HttpGet("hpmis/farmer-list")]
        [EndpointSummary("Get farmer details list from HPMIS")]
        [EndpointDescription("Retrieves farmer application details from HPMIS and filtered by district, sub-district, officer, village or farmer, depending on the search flag.")]
        public async Task<IActionResult> GetFarmerList([FromQuery] FarmerListQueryParams query)
        {
            if (string.IsNullOrWhiteSpace(query.SearchFlag))
                return BadRequest("searchFlag is required.");

            if (string.IsNullOrWhiteSpace(query.FinancialYear.ToString()))
                return BadRequest("financialYear is required.");

            var validFlags = new[] { "DIST", "SUBDIST", "OFFICER", "VILL", "FARMER" };
            if (!validFlags.Contains(query.SearchFlag, StringComparer.OrdinalIgnoreCase))
                return BadRequest($"Invalid searchFlag. Must be one of: {string.Join(", ", validFlags)}");

            if (query.Id is null)
                return BadRequest("id is required.");

            try
            {
                var result = await repository.GetFarmerListByIdAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load farmer list for searchFlag={SearchFlag}, id={Id}", query.SearchFlag, query.Id);
                return Problem("Failed to load farmer details.", statusCode: 500);
            }
        }
    }
}

