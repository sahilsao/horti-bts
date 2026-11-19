using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Officers;
using HortiBts.Shared.Dtos.Officers;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Officers
{
    [Route("api/officers")]
    [ApiController]
    public class OfficersController(IOfficersRepository officersRepository, IHttpContextAccessor httpContextAccessor, ILogger<OfficersController> logger) : ControllerBase
    {
        [HttpGet("get-all-dept-officers")]
        [EndpointSummary("Get all department officers from state")]
        [EndpointDescription("Retrieves the list of all department officers of the state available in the system.")]
        public async Task<IActionResult> GetAllDepartmentOfficersList([FromQuery] int departmentCode)
        {
            try
            {
                var result = await officersRepository.GetDepartmentOfficersListByAsync(departmentCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving dept officers");
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

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

        [HttpGet("get-rheo-officer-previously-available-villages")]
        [EndpointSummary("Get all previously available villages of rheo officer")]
        [EndpointDescription("Retrieves the list of all  previously available villages of rheo officer available in the system.")]
        public async Task<IActionResult> GetRheoOfficerPreviousAvailableVillages([FromQuery] int officerCode, [FromQuery] int flag)
        {
            try
            {
                var result = await officersRepository.GetRheoOfficerPreviousAvailableVillagesListAsync(officerCode, flag);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving previously available villages of rheo officer: {officerCode}", officerCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpGet("get-rheo-officer-list-for-charge-transfer")]
        [EndpointSummary("Get all rheo officer available for charge transfer")]
        [EndpointDescription("Retrieves the list of all rheo officer available for charge transfer in the system.")]
        public async Task<IActionResult> GetRheoOfficersListForChargeTranser([FromQuery] int departmentCode, [FromQuery] int districtCode)
        {
            try
            {
                var result = await officersRepository.GetRheoOfficersListForChargeTranserAsync(departmentCode, districtCode);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving rheo officer available for charge transfer: {districtCode}", districtCode);
                return Problem(detail: "An unexpected error occurred while processing your request.", statusCode: 500);
            }
        }

        [HttpPost("save-officer")]
        [EndpointSummary("Add a new RHEO officer")]
        [EndpointDescription("Inserts the officer and maps the selected villages in one transaction.")]
        public async Task<IActionResult> SaveOfficer([FromBody] SaveOfficerDto dto)
        {
            try
            {

                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);
                var createdBy = User.Identity?.Name ?? "0";

                var result = await officersRepository.SaveOfficerAsync(dto, createdBy, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);   // Result<int> = new officer_code
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save officer.");
                return Problem(detail: "Failed to save officer.", statusCode: 500);
            }
        }

        [HttpPost("transfer-charge")]
        [EndpointSummary("Transfer villages from one officer to another")]
        [EndpointDescription("Writes village history, moves the mapping and assigns vacant villages in one transaction.")]
        public async Task<IActionResult> TransferCharge([FromBody] SaveOfficerChargeTransferDto dto)
        {
            try
            {
                var hasSource = dto.SourceVillageCodes.Count > 0;

                if (dto.TargetOfficerCode <= 0 || dto.TargetRole <= 0
                    || (hasSource && (dto.SourceOfficerCode <= 0 || dto.SourceRole <= 0
                                      || dto.SourceOfficerCode == dto.TargetOfficerCode))
                    || (!hasSource && dto.VacantVillageCodes.Count == 0))
                {
                    return Problem(detail: "Invalid data.", statusCode: 400);
                }

                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                // Same convention as your other pages: Name holds the numeric UserId
                // (for a District user that is the district code, for Admin it is 100).
                var userId = int.TryParse(User.Identity?.Name, out var uid) ? uid : 0;
                var isAdmin = User.IsInRole("Admin");

                var result = await officersRepository.TransferChargeAsync(dto, userId, isAdmin, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to transfer officer charge.");
                return Problem(detail: "Failed to transfer officer charge.", statusCode: 500);
            }
        }
    }
}
