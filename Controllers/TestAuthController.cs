using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jobs_service_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    // TODO: Remove this controller once the central Identity Service is integrated.
    // Token endpoint stays [AllowAnonymous] so developers can obtain a JWT without an existing token.
    // In non-Development environments the endpoint returns 404 to avoid exposing a signing path in production.
    public TestAuthController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public class TestAuthRequest
    {
        public int StudentId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "Student"; // "Student" or "Manager"
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult GenerateToken([FromBody] TestAuthRequest request)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];

        if (string.IsNullOrWhiteSpace(key))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "JWT Key is not configured.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, request.StudentId.ToString()),
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.Role, request.Role),
            // Duplicate StudentId as custom claim for compatibility, if needed
            new("StudentId", request.StudentId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = tokenString });
    }

    [HttpGet("manager-token")]
    [AllowAnonymous]
    public IActionResult GenerateManagerToken()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        return GenerateToken(new TestAuthRequest
        {
            StudentId = 999001,
            Username = "dev-manager",
            Role = "Manager"
        });
    }
}

