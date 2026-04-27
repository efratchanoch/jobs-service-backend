using jobs_service_backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers;

[ApiController]
[Route("api/dev-seed")]
[AllowAnonymous]
public class DevSeedController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DevSeedController> _logger;

    public DevSeedController(AppDbContext dbContext, IWebHostEnvironment environment, ILogger<DevSeedController> logger)
    {
        _dbContext = dbContext;
        _environment = environment;
        _logger = logger;
    }

    [HttpPost("reseed")]
    public async Task<IActionResult> Reseed(CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var removed = await DevelopmentDataSeeder.ClearSeedDataAsync(_dbContext, cancellationToken);
        await DevelopmentDataSeeder.SeedAsync(_dbContext, _logger, cancellationToken);

        return Ok(new
        {
            message = "Development seed data recreated successfully.",
            removedRecords = removed
        });
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var removed = await DevelopmentDataSeeder.ClearSeedDataAsync(_dbContext, cancellationToken);
        return Ok(new
        {
            message = "Development seed data removed.",
            removedRecords = removed
        });
    }
}
