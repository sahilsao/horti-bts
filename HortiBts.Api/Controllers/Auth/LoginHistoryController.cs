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
        public async Task<IActionResult> GetActiveLogin(int userId, string userType)
        {
            var login = await repository.GetActiveLoginAsync(userId, userType);

            if (login is null)
                return Ok(Result<LoginHistoryRecord>.Failure("No active login found."));

            return Ok(Result<LoginHistoryRecord>.Success(login));
        }

        [HttpGet("is-logged-in")]
        public async Task<IActionResult> IsAlreadyLoggedIn(int userId)
        {
            var result = await repository.IsAlreadyLoggedInAsync(userId);
            return Ok(Result<bool>.Success(result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(long loginHistoryId)
        {
            await repository.LogoutAsync(loginHistoryId);
            return Ok(Result.Success());
        }
    }
}
