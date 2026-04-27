using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Manager")]
public class AdminController : ControllerBase
{
    private readonly IAdminStatsService _adminStatsService;

    public AdminController(IAdminStatsService adminStatsService)
    {
        _adminStatsService = adminStatsService;
    }

    [HttpGet("stats")]
    [ProducesResponseType(typeof(AdminStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
    {
        var stats = await _adminStatsService.GetDashboardStatsAsync(cancellationToken);
        return Ok(stats);
    }
}
