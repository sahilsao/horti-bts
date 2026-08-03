using HortiBts.Api.Repository.Auth;
using HortiBts.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await authRepository.LoginAsync(request.Username, request.Password);
            return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            var result = await authRepository.ChangePasswordAsync(request.CurrentPassword, request.NewPassword);
            return result.IsSuccess ? Ok(result.Value) : Problem(detail: result.Error, statusCode: 500);
        }

        [HttpPost("admin-reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminResetPassword(AdminResetPasswordDto request)
        {
            var result = await authRepository.AdminResetPasswordAsync(request.UserId, request.NewPassword);
            return result.IsSuccess ? Ok(result.Value) : Problem(detail: result.Error, statusCode: 500);
        }

        [HttpGet("district-accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDistrictAccounts()
        {
            var result = await authRepository.GetDistrictAccountsAsync();
            return result.IsSuccess ? Ok(result.Value) : Problem(detail: result.Error, statusCode: 500);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await authRepository.RefreshTokenAsync(dto.RefreshToken);
            return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto? dto)
        {
            await authRepository.LogoutAsync(dto?.RefreshToken);
            return Ok();
        }
    }
}
