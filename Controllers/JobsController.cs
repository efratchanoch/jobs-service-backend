using jobs_service_backend.BLL.Repositories.Services; // הכתובת המעודכנת
using jobs_service_backend.DTOs.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace jobs_service_backend.Controllers
{
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

        [HttpGet]
        [Authorize(Roles = "Student,Manager")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _jobService.GetAllPublicJobsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Student,Manager")]
        public async Task<IActionResult> Search([FromQuery] JobSearchFiltersDto filters)
        {
            var result = await _jobService.SearchJobsAsync(filters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student,Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound($"המשרה עם מזהה {id} לא נמצאה.");
            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] CreateJobDto dto)
        {
            var createdJob = await _jobService.CreateJobAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdJob.JobId }, createdJob);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobDto dto)
        {
            var success = await _jobService.UpdateJobAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

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