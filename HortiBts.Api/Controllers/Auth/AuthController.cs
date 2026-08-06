using HortiBts.Api.Repositories.Auth;
using HortiBts.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthRepository authRepository) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await authRepository.LoginAsync(
            request.LoginType,
            request.UserId,
            request.Password);

        return result.IsSuccess
            ? Ok(result.Data)
            : Unauthorized(result.Error);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await authRepository.RefreshTokenAsync(dto.RefreshToken);

        return result.IsSuccess
            ? Ok(result.Data)
            : Unauthorized(result.Error);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto? dto)
    {
        await authRepository.LogoutAsync(dto?.RefreshToken);
        return Ok();
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var result = await authRepository.ChangePasswordAsync(
            request.CurrentPassword,
            request.NewPassword);

        return result.IsSuccess
            ? Ok(result.Data)
            : Problem(detail: result.Error, statusCode: 500);
    }

    [HttpPost("admin-reset-password")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordDto request)
    {
        var result = await authRepository.AdminResetPasswordAsync(
            request.UserId,
            request.NewPassword);

        return result.IsSuccess
            ? Ok(result.Data)
            : Problem(detail: result.Error, statusCode: 500);
    }
}