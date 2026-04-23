using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers;

[ApiController]
[Route("api/mock/admin")]
[AllowAnonymous]
public class MockAdminController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public MockAdminController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        return Ok(new
        {
            totalJobs = 24,
            openJobs = 11,
            closedJobs = 5,
            placementsCompleted = 8
        });
    }

    [HttpPost("jobs")]
    public IActionResult CreateMockJob([FromBody] object payload)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        return Ok(new
        {
            message = "Mock job created successfully.",
            createdAt = DateTime.UtcNow,
            payload
        });
    }
}
