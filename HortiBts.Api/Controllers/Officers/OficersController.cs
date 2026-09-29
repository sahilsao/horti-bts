using HortiBts.Api.Repositories.Officers;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Officers
{
    [Route("api/officers")]
    [ApiController]
    public class OfficersController(IOfficersRepository officersRepository, ILogger<OfficersController> logger) : ControllerBase
    {
        [HttpGet("get-all-rheo-officers")]
        [EndpointSummary("Get all rheo officers from state")]
        [EndpointDescription("Retrieves the list of all rheo officers of the state available in the system.")]
        public async Task<IActionResult> GetAllRheoOfficersList([FromQuery] int departmentCode)
        {
            try
            {
                var result = await officersRepository.GetAllOfficersListByAsync(departmentCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving rheo officers");
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-rheo-officers-from-district")]
        [EndpointSummary("Get all rheo officers from district")]
        [EndpointDescription("Retrieves the list of all rheo officers of the district available in the system.")]
        public async Task<IActionResult> GetRheoOfficersListFromDistrictCode([FromQuery] int departmentCode, [FromQuery] int districtCode)
        {
            try
            {
                var result = await officersRepository.GetOfficersListByDistrictCodeAsync(departmentCode, districtCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving rheo officers for district code: {DistrictCode}", districtCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-rheo-officers-from-sub-district")]
        [EndpointSummary("Get all rheo officers from sub-district")]
        [EndpointDescription("Retrieves the list of all rheo officers of the sub-district of the district available in the system.")]
        public async Task<IActionResult> GetRheoOfficersListFromSubDistrictCode([FromQuery] int departmentCode, [FromQuery] int subDistrictCode)
        {
            try
            {
                var result = await officersRepository.GetOfficersListBySubDistrictCodeAsync(departmentCode, subDistrictCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving rheo officers for sub-district code: {SubDistrictCode}", subDistrictCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-rheo-officers-from-village")]
        [EndpointSummary("Get all rheo officers from village")]
        [EndpointDescription("Retrieves the list of all rheo officers of the village of the sub-district available in the system.")]
        public async Task<IActionResult> GetRheoOfficersListFromVillageCode([FromQuery] int departmentCode, [FromQuery] int villageCode)
        {
            try
            {
                var result = await officersRepository.GetOfficersListByVillageCodeAsync(departmentCode, villageCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving rheo officers for village code: {SubDistrictCode}", villageCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-rheo-officer-mapped-villages")]
        [EndpointSummary("Get all mapped villages of rheo officer")]
        [EndpointDescription("Retrieves the list of all mapped villages of rheo officer available in the system.")]
        public async Task<IActionResult> GetRheoOfficerMappedVillages([FromQuery] int departmentCode, [FromQuery] int officerCode)
        {
            try
            {
                var result = await officersRepository.GetRheoOfficerMappedVillagesListAsync(departmentCode, officerCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving mapped villages of rheo officer: {officerCode}", officerCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }
    }
}
