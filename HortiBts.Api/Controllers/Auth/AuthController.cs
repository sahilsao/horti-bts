using HortiBts.Api.Repositories.Auth;
using HortiBts.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthRepository authRepository, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [EndpointSummary("User login")]
    [EndpointDescription("Authenticates a user using their login type, user ID, and password, and returns authentication tokens on successful login.")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await authRepository.LoginAsync(
                request.LoginType,
                request.UserId,
                request.Password);

            return result.IsSuccess
                ? Ok(result.Data)
                : Unauthorized(result.Error);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during login for user {UserId}.", request.UserId);
            return Problem(detail: "An unexpected error occurred. Please try again later.", statusCode: 500);
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [EndpointSummary("Refresh access token")]
    [EndpointDescription("Generates a new access token using a valid refresh token.")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        try
        {
            var result = await authRepository.RefreshTokenAsync(dto.RefreshToken);

            return result.IsSuccess
                ? Ok(result.Data)
                : Unauthorized(result.Error);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while refreshing the token.");
            return Problem(detail: "An unexpected error occurred. Please try again later.", statusCode: 500);
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [EndpointSummary("User logout")]
    [EndpointDescription("Logs out the currently authenticated user and invalidates the supplied refresh token.")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto? dto)
    {
        try
        {
            await authRepository.LogoutAsync(dto?.RefreshToken);

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during logout.");
            return Problem(detail: "An unexpected error occurred. Please try again later.", statusCode: 500);
        }
    }

    [HttpPost("admin-change-password")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Change password")]
    [EndpointDescription("Allows the currently authenticated user to change their password by providing the current password and a new password.")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        try
        {
            var result = await authRepository.ChangePasswordAsync(
                request.CurrentPassword,
                request.NewPassword);

            return result.IsSuccess
                ? Ok(result.Data)
                : Problem(detail: result.Error, statusCode: 500);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while changing the password.");
            return Problem(detail: "An unexpected error occurred. Please try again later.", statusCode: 500);
        }
    }

    [HttpPost("admin-reset-password")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Reset user password")]
    [EndpointDescription("Allows an administrator to reset the password of a specified user.")]
    public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordDto request)
    {
        try
        {
            var result = await authRepository.AdminResetPasswordAsync(
                request.UserId,
                request.NewPassword);

            return result.IsSuccess
                ? Ok(result.Data)
                : Problem(detail: result.Error, statusCode: 500);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while resetting the password for user {UserId}.", request.UserId);
            return Problem(detail: "An unexpected error occurred. Please try again later.", statusCode: 500);
        }
    }
}