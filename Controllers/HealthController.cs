using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace jobs_service_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "OK" });
}

