using HortiBts.Api.Repositories.Reports.BacklogYearly;
using HortiBts.Shared.Dtos.Reports.BacklogYearly;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.BacklogYearly
{
    [Route("api/reports/backlog")]
    [ApiController]
    public class BacklogYearlyFarmerRegistrationController(IBacklogFarmerRegistrationRepository repository,
        ILogger<DistWiseFarmerRegistrationDto> logger,
        ILogger<BlockWiseFarmerRegistrationDto> logger2,
        ILogger<RheoWiseFarmerRegistrationDto> logger3,
        ILogger<VillageWiseFarmerRegistrationDto> logger4,
        ILogger<FarmerWiseFarmerRegistrationDto> logger5) : ControllerBase
    {
        [HttpGet("distwise-farmer-registration")]
        [EndpointSummary("Get backlog report of district-wise farmer registration")]
        [EndpointDescription("Retrieves the backlog report of district-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfDistWiseFarmerRegistrationAsync([FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetBacklogRptOfDistWiseFarmerRegistrationAsync(financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load backlog report of district-wise farmer registration.");
                return Problem(detail: "Failed to load backlog report of district-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("blockwise-farmer-registration")]
        [EndpointSummary("Get backlog report of block-wise farmer registration")]
        [EndpointDescription("Retrieves the backlog report of block-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfBlockWiseFarmerRegistrationAsync([FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetBacklogRptOfBlockWiseFarmerRegistrationAsync(districtCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load backlog report of block-wise farmer registration.");
                return Problem(detail: "Failed to load backlog report of block-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("rheowise-farmer-registration")]
        [EndpointSummary("Get backlog yearly report of rheo-wise farmer registration")]
        [EndpointDescription("Retrieves backlog the yearly report of rheo-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfRheoWiseFarmerRegistrationAsync([FromQuery] int departmentCode, [FromQuery] int districtCode, [FromQuery] int subDistrictCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetBacklogRptOfRheoWiseFarmerRegistrationAsync(new RheoWiseFarmerFilterDto
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
                logger3.LogError(ex, "Failed to load backlog yearly report of rheo-wise farmer registration.");
                return Problem(detail: "Failed to load backlog yearly report of rheo-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("villagewise-farmer-registration")]
        [EndpointSummary("Get backlog yearly report of village-wise farmer registration")]
        [EndpointDescription("Retrieves the backlog yearly report of village-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfVillageWiseFarmerRegistrationAsync([FromQuery] int officerCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetBacklogRptOfVillageWiseFarmerRegistrationAsync(officerCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger4.LogError(ex, "Failed to load backlog yearly report of village-wise farmer registration.");
                return Problem(detail: "Failed to load backlog yearly report of village-wise farmer registration.", statusCode: 500);
            }
        }

        [HttpGet("farmerwise-farmer-registration")]
        [EndpointSummary("Get yearly backlog report of farmer-wise farmer registration")]
        [EndpointDescription("Retrieves the backlog yearly report of farmer-wise farmer registration for a specified financial year.")]
        public async Task<IActionResult> GetBacklogRptOfFarmerWiseFarmerRegistrationAsync(
           [FromQuery] int departmentCode,
           [FromQuery] int districtCode,
           [FromQuery] int subDistrictCode,
           [FromQuery] int villageCode,
           [FromQuery] int officerCode,
           [FromQuery] int financialYear,
           [FromQuery] int schemeTypeId,
           [FromQuery] int schemeId,
           [FromQuery] int componentId
       )
        {
            try
            {
                var result = await repository.GetBacklogRptOfFarmerWiseFarmerRegistrationAsync(new FarmerWiseReportFilterDto
                {
                    DepartmentCode = departmentCode,
                    DistrictCode = districtCode,
                    SubDistrictCode = subDistrictCode,
                    VillageCode = villageCode,
                    OfficerCode = officerCode,
                    FinancialYear = financialYear,
                    SchemeType = schemeTypeId,
                    SchemeId = schemeId,
                    ComponentId = componentId
                });
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger5.LogError(ex, "Failed to load backlog yearly report of farmer-wise farmer registration.");
                return Problem(detail: "Failed to load backlog yearly report of farmer-wise farmer registration.", statusCode: 500);
            }
        }
    }
}
