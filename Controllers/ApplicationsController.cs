using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.BLL.Services;
using jobs_service_backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IIdentityService _identityService;

        public ApplicationsController(IApplicationService applicationService, IIdentityService identityService)
        {
            _applicationService = applicationService;
            _identityService = identityService;
        }

        [HttpGet("my")]
public async Task<IActionResult> GetMyApplications(
    [FromQuery] List<ApplicationStatus>? statuses = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
{
    try
    {
        var studentId = _identityService.GetStudentId(User);
        var result = await _applicationService.GetMyApplicationsAsync(studentId, statuses, pageNumber, pageSize);
        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
}

[HttpGet("job/{jobId:int}")]
[Authorize(Roles = "Manager")]
public async Task<IActionResult> GetApplicationsForJob(
    int jobId,
    [FromQuery] List<ApplicationStatus>? statuses = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
{
    try
    {
        var result = await _applicationService.GetApplicationsForJobAsync(jobId, statuses, pageNumber, pageSize);
        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
}

        /// <summary>
        /// הגשת מועמדות למשרה (studentId מה-Claims, לא מה-body).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ApplyToJob([FromBody] CreateApplicationDto dto)
        {
            try
            {
                var studentId = _identityService.GetStudentId(User);
                var result = await _applicationService.ApplyToJobAsync(dto, studentId);
                return CreatedAtAction(nameof(GetMyApplications), null, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// עדכון סטטוס מועמדות – למנהלת בלבד.
        /// </summary>
        [HttpPut("status")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusDto dto)
        {
            try
            {
                var success = await _applicationService.UpdateApplicationStatusAsync(dto);
                if (!success)
                    return NotFound($"הגשה עם מזהה {dto.ApplicationId} לא נמצאה.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// עדכון הערות פנימיות להגשה – למנהלת בלבד.
        /// </summary>
        [HttpPut("{applicationId:int}/notes")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateNotes(int applicationId, [FromBody] UpdateNotesDto dto)
        {
            try
            {
                var success = await _applicationService.UpdateNotesAsync(applicationId, dto?.Notes);
                if (!success)
                    return NotFound($"הגשה עם מזהה {applicationId} לא נמצאה.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
