using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.Districts;
using HortiBts.Api.Repositories.Users;
using HortiBts.Shared.Dtos.Users;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUsersRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<UsersController> logger) : ControllerBase
    {
        [HttpGet("district/get-login-users")]
        [EndpointSummary("Get all districts users")]
        [EndpointDescription("Retrieves the list of all districts login users available in the system.")]
        public async Task<IActionResult> GetDistrictsUsers(int departmentCode, int userType)
        {
            try
            {
                var result = await repository.GetDistrictsUsersAsync(departmentCode, userType);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving districts users.");
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }
        }

        [HttpPost("district/reset-password")]
        public async Task<IActionResult> DistrictResetPassword([FromQuery] int districtCode, [FromQuery] int departmentCode, [FromQuery] int userType)
        {
            try
            {
                var defaultHash = "$2y$10$bxLphQHZp96xv8mFT6iicutES5GmXmQh2W.SsDWiydIxtv7qNOjd.";
                var PasswordFlag = 0;

                if (string.IsNullOrWhiteSpace(defaultHash))
                    return StatusCode(500, "Reset password is not configured.");

                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await repository.DistrictResetPasswordAsync(
                    districtCode, departmentCode, userType,
                    defaultHash, PasswordFlag, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while resetting district password.");
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }
        }

        [HttpPost("shdo/reset-password")]
        public async Task<IActionResult> ShdoResetPassword([FromQuery] int officerCode, [FromQuery] int userType)
        {
            try
            {
                var defaultHash = "$2y$10$bxLphQHZp96xv8mFT6iicutES5GmXmQh2W.SsDWiydIxtv7qNOjd.";
                var PasswordFlag = 0;

                if (string.IsNullOrWhiteSpace(defaultHash))
                    return StatusCode(500, "Reset password is not configured.");

                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await repository.ShdoResetPasswordAsync(
                    officerCode, userType,
                    defaultHash, PasswordFlag, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while resetting shdo password.");
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }
        }

        [HttpPost("rheo/reset-password")]
        public async Task<IActionResult> RheoResetPassword([FromQuery] int officerCode, [FromQuery] int userType)
        {
            try
            {
                var defaultHash = "$2y$10$bxLphQHZp96xv8mFT6iicutES5GmXmQh2W.SsDWiydIxtv7qNOjd.";
                var PasswordFlag = 0;

                if (string.IsNullOrWhiteSpace(defaultHash))
                    return StatusCode(500, "Reset password is not configured.");

                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await repository.RheoResetPasswordAsync(
                    officerCode, userType,
                    defaultHash, PasswordFlag, clientIp);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while resetting Rheo password.");
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }
        }
    }
}
