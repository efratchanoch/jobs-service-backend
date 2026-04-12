using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.BLL.Services;
using jobs_service_backend.DTOs.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorage;
    private readonly IApplicationRepository _applications;
    private readonly IIdentityService _identity;

    public FilesController(
        IFileStorageService fileStorage,
        IApplicationRepository applications,
        IIdentityService identity)
    {
        _fileStorage = fileStorage;
        _applications = applications;
        _identity = identity;
    }

    /// <summary>העלאת תמונה למשרה. מחזיר URL לשמירה בשדה JobImageUrl ביצירה/עדכון משרה (מנהלת).</summary>
    [HttpPost("job-image")]
    [Authorize(Roles = "Manager")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadedFileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadJobImage(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            var relative = await _fileStorage.SaveJobImageAsync(file, cancellationToken);
            var url = BuildPublicUrl(relative);
            return Ok(new UploadedFileResponseDto { Url = url });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>העלאת קורות חיים. מחזיר כתובת מלאה ל-<c>GET api/files/resume/&#123;fileName&#125;</c> לשמירה בשדה ResumeUrl (תלמידה).</summary>
    [HttpPost("resume")]
    [RequestSizeLimit(15 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadedFileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadResume(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            var fileName = await _fileStorage.SaveResumeAsync(file, cancellationToken);
            var url = BuildResumeDownloadUrl(fileName);
            return Ok(new UploadedFileResponseDto { Url = url });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>הורדת קורות חיים (מנהלת או תלמידה שבבעלותה הקובץ לפי ResumeUrl בהגשה).</summary>
    [HttpGet("resume/{fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DownloadResume(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        if (string.IsNullOrEmpty(fileName)) return BadRequest();

        var allowed = false;
        if (User.IsInRole("Manager"))
            allowed = await _applications.ManagerCanAccessResumeFileAsync(fileName);
        else
        {
            int studentId;
            try
            {
                studentId = _identity.GetStudentId(User);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

            allowed = await _applications.StudentOwnsResumeFileAsync(fileName, studentId);
        }

        if (!allowed) return NotFound();

        var path = _fileStorage.GetResumePhysicalPath(fileName);
        if (path == null) return NotFound();

        return PhysicalFile(path, GetResumeContentType(fileName), fileDownloadName: fileName);
    }

    private string BuildPublicUrl(string relativePath)
    {
        var req = HttpContext.Request;
        return $"{req.Scheme}://{req.Host}{req.PathBase}{relativePath}";
    }

    private string BuildResumeDownloadUrl(string fileName)
    {
        var req = HttpContext.Request;
        return $"{req.Scheme}://{req.Host}{req.PathBase}/api/files/resume/{fileName}";
    }

    private static string GetResumeContentType(string fileName)
    {
        return Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }
}
