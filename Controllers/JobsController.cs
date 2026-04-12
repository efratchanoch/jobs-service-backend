using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
    /// <summary>
    /// Public job listings and manager CRUD for jobs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>
        /// Returns a paginated list of public jobs, optionally filtered by status and sort order.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Student,Manager")]
        public async Task<IActionResult> GetAll(
            [FromQuery] List<JobStatus>? statuses = null,
            [FromQuery] bool newestFirst = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _jobService.GetAllPublicJobsAsync(statuses, newestFirst, pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Searches and filters jobs (tags, text, location, etc.) with pagination.
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = "Student,Manager")]
        public async Task<IActionResult> Search([FromQuery] JobSearchFiltersDto filters)
        {
            var result = await _jobService.SearchJobsAsync(filters);
            return Ok(result);
        }

        /// <summary>
        /// Gets a single job by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Student,Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
                return NotFound($"Job with id {id} was not found.");
            return Ok(job);
        }

        /// <summary>
        /// Creates a job. Manager role required.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] CreateJobDto dto)
        {
            var createdJob = await _jobService.CreateJobAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdJob.JobId }, createdJob);
        }

        /// <summary>
        /// Updates an existing job. Manager role required.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobDto dto)
        {
            var success = await _jobService.UpdateJobAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a job. Manager role required.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _jobService.DeleteJobAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
