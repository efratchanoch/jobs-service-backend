using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.BLL.Services;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs;
using jobs_service_backend.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
    /// <summary>
    /// Job applications: students submit and list their applications; managers review applications per job.
    /// </summary>
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

        /// <summary>
        /// Returns the authenticated student's applications, paginated and optionally filtered by status.
        /// </summary>
        /// <param name="statuses">Optional status filter; omit to include all.</param>
        /// <param name="newestFirst">When true, sorts by <c>AppliedAt</c> descending.</param>
        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(PaginatedListDto<StudentApplicationsListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMyApplications(
            [FromQuery] List<ApplicationStatus>? statuses = null,
            [FromQuery] bool newestFirst = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var studentId = _identityService.GetStudentId(User);
                var result = await _applicationService.GetMyApplicationsAsync(studentId, statuses, newestFirst, pageNumber, pageSize);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns all applications for a specific job. Manager role required.
        /// </summary>
        [HttpGet("job/{jobId:int}")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(typeof(PaginatedListDto<JobApplicationsListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetApplicationsForJob(
            int jobId,
            [FromQuery] List<ApplicationStatus>? statuses = null,
            [FromQuery] bool newestFirst = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _applicationService.GetApplicationsForJobAsync(jobId, statuses, newestFirst, pageNumber, pageSize);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Submits an application to a job. The student id is taken from JWT claims, not from the request body.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(StudentApplicationsListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// Updates application status (and optional notes). Manager role required.
        /// </summary>
        [HttpPut("status")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusDto dto)
        {
            try
            {
                var success = await _applicationService.UpdateApplicationStatusAsync(dto);
                if (!success)
                    return NotFound($"Application with id {dto.ApplicationId} was not found.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates internal manager notes on an application. Manager role required.
        /// </summary>
        [HttpPut("{applicationId:int}/notes")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNotes(int applicationId, [FromBody] UpdateNotesDto dto)
        {
            try
            {
                var success = await _applicationService.UpdateNotesAsync(applicationId, dto?.Notes);
                if (!success)
                    return NotFound($"Application with id {applicationId} was not found.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
