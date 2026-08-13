using HortiBts.Api.Models.Auth;
using HortiBts.Api.Repositories.Auth;
using HortiBts.Shared.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Auth
{
    [Route("api/login-history")]
    [ApiController]
    public class LoginHistoryController(ILoginHistoryRepository repository) : ControllerBase
    {
        [HttpGet("active")]
        [EndpointSummary("Get active login")]
        [EndpointDescription("Retrieves the active login history record for a user based on their user ID and user type.")]
        public async Task<IActionResult> GetActiveLogin(int userId, string userType)
        {
            var login = await repository.GetActiveLoginAsync(userId, userType);

            if (login is null)
                return Ok(Result<LoginHistoryRecord>.Failure("No active login found."));

            return Ok(Result<LoginHistoryRecord>.Success(login));
        }

        [HttpGet("is-logged-in")]
        [EndpointSummary("Check user login status")]
        [EndpointDescription("Checks whether a user is currently logged in based on their user ID.")]
        public async Task<IActionResult> IsAlreadyLoggedIn(int userId)
        {
            var result = await repository.IsAlreadyLoggedInAsync(userId);

            return Ok(Result<bool>.Success(result));
        }

        [HttpPost("logout")]
        [EndpointSummary("Logout user")]
        [EndpointDescription("Logs out a user by updating their login history using the specified login history ID.")]
        public async Task<IActionResult> Logout(long loginHistoryId)
        {
            await repository.LogoutAsync(loginHistoryId);

            return Ok(Result.Success());
        }
    }
}