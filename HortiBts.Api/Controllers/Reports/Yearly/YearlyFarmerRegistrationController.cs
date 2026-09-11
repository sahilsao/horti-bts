using HortiBts.Api.Repositories.Reports.Yearly;
using HortiBts.Shared.Dtos.Reports;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.Yearly
{
    [Route("api/reports/yearly")]
    [ApiController]
    public class YearlyFarmerRegistrationController(IYearlyFarmerRegistrationRepository repository,
        ILogger<DistwiseFarmerRegistrationDto> logger,
        ILogger<BlockwiseFarmerRegistrationDto> logger2) : ControllerBase
    {
        [HttpGet("distwise-farmer-registration")]
        [EndpointSummary("Get yearly report of district-wise farmer registration")]
        [EndpointDescription("Retrieves the yearly report of district-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetYearlyRptOfDistwiseFarmerRegistrationAsync([FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetYearlyRptOfDistwiseFarmerRegistrationAsync(financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load yearly report of district-wise farmer registration.");
                return Problem(detail: "Failed to load yearly report of district-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("blockwise-farmer-registration")]
        [EndpointSummary("Get yearly report of block-wise farmer registration")]
        [EndpointDescription("Retrieves the yearly report of block-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetYearlyRptOfBlockwiseFarmerRegistrationAsync([FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetYearlyRptOfBlockwiseFarmerRegistrationAsync(districtCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load yearly report of block-wise farmer registration.");
                return Problem(detail: "Failed to load yearly report of block-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("rheowise-farmer-registration")]
        [EndpointSummary("Get yearly report of rheo-wise farmer registration")]
        [EndpointDescription("Retrieves the yearly report of rheo-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetYearlyRptOfRheowiseFarmerRegistrationAsync([FromQuery] int departmentCode, [FromQuery] int districtCode, [FromQuery] int subDistrictCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetYearlyRptOfRheowiseFarmerRegistrationAsync(new RheoWiseFarmerFilterDto
                {
                    DepartmentCode = departmentCode,
                    DistrictCode = districtCode,
                    SubDistrictCode = subDistrictCode,
                    FinancialYear = financialYear
                });
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load yearly report of rheo-wise farmer registration.");
                return Problem(detail: "Failed to load yearly report of rheo-wise farmer registration.", statusCode: 500);
            }
        }
    }
}
