using HortiBts.Api.Repositories.Dashboard.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Dashboard.Admin
{
    [Route("api/dashboard/admin")]
    [ApiController]
    public class DashboardController(IDashboardRepository dashboardRepository) : ControllerBase
    {
        [HttpGet("get-tot-rheo-count")]
        [EndpointSummary("Get total RHEO count")]
        [EndpointDescription("Retrieves the total number of RHEO users registered in the system.")]
        public async Task<IActionResult> GetTotRheoCount()
        {
            var result = await dashboardRepository.GetTotRHEOCount();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }

        [HttpGet("get-tot-reg-farmers-count")]
        [EndpointSummary("Get total registered farmers count")]
        [EndpointDescription("Retrieves the total number of farmers registered in the system.")]
        public async Task<IActionResult> GetTotRegFarmersCount()
        {
            var result = await dashboardRepository.GetTotRegFarmersCount();

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }

        [HttpGet("get-tot-reg-backlog-farmers-count")]
        [EndpointSummary("Get total backlog farmers count")]
        [EndpointDescription("Retrieves the total number of backlog farmers registered for the specified financial year.")]
        public async Task<IActionResult> GetTotRegBacklogFarmersCount(
            [FromQuery] string finYear)
        {
            if (string.IsNullOrWhiteSpace(finYear))
                return Problem(
                    detail: "finYear query parameter is required.",
                    statusCode: 400);

            var result =
                await dashboardRepository.GetTotRegBacklogFarmersCount(finYear);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 500);

            return Ok(result);
        }

        [HttpGet("get-application-dashboard")]
        [EndpointSummary("Get application dashboard")]
        [EndpointDescription("Retrieves application statistics for the specified financial year, including total applications, RHEO approvals, DDH approvals, and state-sponsored and central-sponsored applications.")]
        public async Task<IActionResult> GetApplicationDashboard(
            [FromQuery] string finYear)
        {
            if (string.IsNullOrWhiteSpace(finYear))
            {
                return Problem(
                    detail: "finYear query parameter is required.",
                    statusCode: 400);
            }

            var result =
                await dashboardRepository.GetApplicationDashboard(finYear);

            if (!result.IsSuccess)
            {
                return Problem(
                    detail: result.Error,
                    statusCode: 500);
            }

            return Ok(result);
        }
    }
}