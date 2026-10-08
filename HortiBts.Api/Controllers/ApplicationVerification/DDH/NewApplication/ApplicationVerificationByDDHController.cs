using System.Security.Claims;
using HortiBts.Api.Helpers;
using HortiBts.Api.Repositories.ApplicationVerification.DDH.NewApplication;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.ApplicationVerification.DDH
{
    [Route("api/district/application-verification-by-ddh")]
    public class ApplicationVerificationByDDHController(IApproveRejectFarmerApplicationByDDHRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationVerificationByDDHController> logger) : ControllerBase
    {
        [HttpPost("approve-reject")]
        [EndpointSummary("Approve or reject a farmer application by DDH`")]
        [EndpointDescription("approves or rejects a farmer application by DDH based on the provided application ID, remark, status, IP address, and user ID.")]

        public async Task<IActionResult> ApproveReject([FromBody] ApproveRejectFarmerApplicationRequestDto request)
        {
            try
            {
                var userId = User.Identity?.Name ?? "0";
                var clientIp = IpAddressHelper.GetClientIp(httpContextAccessor);

                var result = await repository.ApproveRejectFarmerApplicationAsync(
                    request.ApplicationId,
                    request.Status,
                    request.Remark,
                    clientIp,
                    userId);

                if (!result.IsSuccess)
                    return Problem(detail: result.Error, statusCode: 500);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to approve or reject farmer application.");
                return Problem("Failed to approve or reject farmer application.", statusCode: 500);
            }
        }
    }
}