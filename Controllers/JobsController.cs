using JobsService.BLL.Services;
using JobsService.DTOs.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace JobsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _jobService.GetAllPublicJobsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] JobSearchFiltersDto filters)
        {
            var result = await _jobService.SearchJobsAsync(filters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound($"המשרה עם מזהה {id} לא נמצאה.");
            return Ok(job);
        }

        [HttpPost]
        // הרשאה: מנהלת (כאן בהמשך תוסיפי [Authorize(Roles = "Admin")])
        public async Task<IActionResult> Create([FromBody] CreateJobDto dto)
        {
            var createdJob = await _jobService.CreateJobAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdJob.JobId }, createdJob);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobDto dto)
        {
            var success = await _jobService.UpdateJobAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _jobService.DeleteJobAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}