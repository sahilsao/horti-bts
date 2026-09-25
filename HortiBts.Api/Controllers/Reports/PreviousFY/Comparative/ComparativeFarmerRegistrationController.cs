using HortiBts.Api.Repositories.Reports.PreviousFY.Comparative;
using HortiBts.Shared.Dtos.Reports.PreviousFY.Comparative;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.PreviousFY.Comparative
{
    [Route("api/reports/comparative/previous")]
    [ApiController]
    public class ComparativeFarmerRegistrationController(IComparativeFarmerRegistrationRepository repository,
        ILogger<DistWiseComparativeFarmerRegistrationDto> logger,
        ILogger<BlockWiseComparativeFarmerRegistrationDto> logger2,
        ILogger<RheoWiseComparativeFarmerRegistrationDto> logger3,
        ILogger<VillageWiseComparativeFarmerRegistrationDto> logger4) : ControllerBase
    {
        [HttpGet("distwise-farmer-registration")]
        [EndpointSummary("Get comparative report of district-wise farmer registration")]
        [EndpointDescription("Retrieves the comparative report of district-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetComparativeRptOfDistWiseFarmerRegistrationAsync([FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfDistWiseComparativeFarmerRegistrationAsync(financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load comparative report of district-wise farmer registration.");
                return Problem(detail: "Failed to load comparative report of district-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("blockwise-farmer-registration")]
        [EndpointSummary("Get comparative report of block-wise farmer registration")]
        [EndpointDescription("Retrieves the comparative report of block-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetComparativeRptOfBlockWiseFarmerRegistrationAsync([FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfBlockWiseComparativeFarmerRegistrationAsync(districtCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load comparative report of block-wise farmer registration.");
                return Problem(detail: "Failed to load comparative report of block-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("rheowise-farmer-registration")]
        [EndpointSummary("Get comparative yearly report of rheo-wise farmer registration")]
        [EndpointDescription("Retrieves comparative the yearly report of rheo-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetComparativeRptOfRheoWiseFarmerRegistrationAsync([FromQuery] int departmentCode, [FromQuery] int districtCode, [FromQuery] int subDistrictCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfRheoWiseComparativeFarmerRegistrationAsync(new RheoWiseFarmerFilterDto
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
                logger3.LogError(ex, "Failed to load comparative yearly report of rheo-wise farmer registration.");
                return Problem(detail: "Failed to load comparative yearly report of rheo-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("villagewise-farmer-registration")]
        [EndpointSummary("Get comparative yearly report of village-wise farmer registration")]
        [EndpointDescription("Retrieves the comparative yearly report of village-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetComparativeRptOfVillageWiseFarmerRegistrationAsync([FromQuery] int officerCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfVillageWiseComparativeFarmerRegistrationAsync(officerCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger4.LogError(ex, "Failed to load comparative yearly report of village-wise farmer registration.");
                return Problem(detail: "Failed to load comparative yearly report of village-wise farmer registration.", statusCode: 500);
            }
        }
    }
}
