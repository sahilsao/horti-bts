using HortiBts.Api.Models.Auth;
using HortiBts.Api.Repositories.Dashboard.Admin;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Dashboard.Admin
{
    [Route("api/dashboard/admin")]
    [ApiController]
    public class DashboardController(IDashboardRepository DashboardRepository) : ControllerBase
    {
        [HttpGet("get-tot-rheo-count")]
        public async Task<IActionResult> GetTotRHEOCount()
        {
            var data = await DashboardRepository.GetTotRHEOCount();

            if (data is null)
                return Ok(Result<DashboardDto>.Failure("No active login found."));

            return Ok(Result<DashboardDto>.Success(data));
        }
    }
}
