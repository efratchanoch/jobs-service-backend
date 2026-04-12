using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.DTOs.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Returns all tags (for filters and search). Public endpoint.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        /// <summary>
        /// Creates a new tag. Manager only.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto dto)
        {
            try
            {
                var created = await _tagService.CreateTagAsync(dto);
                return Created($"/api/tags/{created.TagId}", created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a tag by id. Manager only.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var success = await _tagService.DeleteTagAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
