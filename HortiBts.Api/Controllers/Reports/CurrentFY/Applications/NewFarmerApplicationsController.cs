using HortiBts.Api.Repositories.Reports.CurrentFY.Applications;
using HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Reports.CurrentFY.Applications
{
    [Route("api/reports/applications/current")]
    [ApiController]
    public class NewFarmerApplicationsController(INewFarmerApplicationsRepository repository,
        ILogger<DistrictWiseFarmerApplicationsDto> logger,
        ILogger<BlockWiseFarmerApplicationsDto> logger2,
        ILogger<RHEOWiseFarmerApplicationsDto> logger3,
        ILogger<VillageWiseFarmerApplicationsDto> logger4,
        ILogger<FarmerWiseFarmerApplicationsDto> logger5) : ControllerBase
    {

        [HttpGet("districtwise-list")]
        [EndpointSummary("Get district-wise applications list report")]
        [EndpointDescription("Retrieves the district-wise applications list of farmer applications. filters by fy code.")]
        public async Task<IActionResult> GetApplicationsListRptOfDistrictWiseAsync([FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfDistrictWiseApplicationsAsync(financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load district-wise applications list of farmer applications.");
                return Problem(detail: "Failed to load district-wise applications list of farmer applications.", statusCode: 500);
            }
        }

        [HttpGet("blockwise-list")]
        [EndpointSummary("Get block-wise applications list report")]
        [EndpointDescription("Retrieves the block-wise applications list of farmer applications. filters by districtcode, fy code.")]
        public async Task<IActionResult> GetApplicationsListRptOfBlockWiseAsync([FromQuery] int districtCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfBlockWiseApplicationsAsync(districtCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger2.LogError(ex, "Failed to load block-wise applications list of farmer applications.");
                return Problem(detail: "Failed to load block-wise applications list of farmer applications.", statusCode: 500);
            }
        }

        [HttpGet("rheowise-list")]
        [EndpointSummary("Get RHEO-wise applications list report")]
        [EndpointDescription("Retrieves the RHEO-wise applications list of farmer applications. filters by districtcode, subdistrictcode, fy code.")]
        public async Task<IActionResult> GetApplicationsListRptOfRheoWiseAsync([FromQuery] int departmentCode, [FromQuery] int districtCode, [FromQuery] int subDistrictCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfRheoWiseApplicationsAsync(departmentCode, districtCode, subDistrictCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger3.LogError(ex, "Failed to load RHEO-wise applications list of farmer applications.");
                return Problem(detail: "Failed to load RHEO-wise applications list of farmer applications.", statusCode: 500);
            }
        }

        [HttpGet("villagewise-list")]
        [EndpointSummary("Get village-wise applications list report")]
        [EndpointDescription("Retrieves the village-wise applications list of farmer applications. filters by oficercode & fy code.")]
        public async Task<IActionResult> GetApplicationsListRptOfVillageWiseAsync([FromQuery] int officerCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfVillageWiseApplicationsAsync(officerCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger4.LogError(ex, "Failed to load village-wise applications list of farmer applications.");
                return Problem(detail: "Failed to load village-wise applications list of farmer applications.", statusCode: 500);
            }
        }

        [HttpGet("farmerwise-list")]
        [EndpointSummary("Get farmer-wise applications list report")]
        [EndpointDescription("Retrieves the farmer-wise applications list of farmer applications. Optionally filters by district/subdistrict,officercode/fy code.")]
        public async Task<IActionResult> GetApplicationsListRptOfFarmerWiseAsync([FromQuery] int districtCode, [FromQuery] int subDistrictCode, [FromQuery] int villageCode, [FromQuery] int officerCode, [FromQuery] int financialYear)
        {
            try
            {
                var result = await repository.GetRptOfFarmerWiseApplicationsAsync(districtCode, subDistrictCode, villageCode, officerCode, financialYear);
                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger5.LogError(ex, "Failed to load farmer-wise applications list of farmer applications.");
                return Problem(detail: "Failed to load farmer-wise applications list of farmer applications.", statusCode: 500);
            }
        }
    }
}
