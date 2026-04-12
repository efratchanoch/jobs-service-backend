using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.BLL.Services;
using jobs_service_backend.DTOs.Common;
using jobs_service_backend.DTOs.Invitations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
    /// <summary>
    /// Private job invitations: managers invite students to private listings; students list and acknowledge invitations.
    /// </summary>
    /// <remarks>
    /// Student-scoped operations resolve the caller from the JWT (<c>NameIdentifier</c> or <c>StudentId</c> claim via <see cref="IIdentityService"/>).
    /// Bulk send requires the <c>Manager</c> role.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvitationsController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly IIdentityService _identityService;

        public InvitationsController(IInvitationService invitationService, IIdentityService identityService)
        {
            _invitationService = invitationService;
            _identityService = identityService;
        }

        /// <summary>
        /// Sends private job invitations to multiple students for a single job (manager only).
        /// </summary>
        /// <param name="dto">Target job and list of student identifiers.</param>
        /// <returns>No content when invitations are persisted.</returns>
        /// <response code="204">Invitations created successfully.</response>
        /// <response code="400">Validation failed on the request body.</response>
        /// <response code="401">Missing or invalid authentication.</response>
        /// <response code="403">Caller is not in the Manager role.</response>
        [HttpPost("bulk")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> SendBulkInvitations([FromBody] BulkInvitationDto dto)
        {
            await _invitationService.SendInvitationsAsync(dto.JobId, dto.StudentIds);

            // TODO: optional email notifications to invited students
            return NoContent();
        }

        /// <summary>
        /// Returns all private invitations for the authenticated student, newest first, paginated.
        /// </summary>
        /// <param name="pageNumber">1-based page index (default 1).</param>
        /// <param name="pageSize">Page size (default 10).</param>
        /// <returns>
        /// A <see cref="PaginatedListDto{T}"/> of <see cref="InvitationDto"/> including both viewed and unviewed invitations.
        /// </returns>
        /// <response code="200">Paged list of invitations.</response>
        /// <response code="401">Missing or invalid JWT, or student id cannot be resolved from claims.</response>
        [HttpGet("my/all")]
        [ProducesResponseType(typeof(PaginatedListDto<InvitationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyPrivateInvitations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var studentId = _identityService.GetStudentId(User);
            var result = await _invitationService.GetMyInvitationsAsync(studentId, pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Returns only <strong>new</strong> private invitations for the authenticated student (not yet marked as viewed), newest first, paginated.
        /// </summary>
        /// <param name="pageNumber">1-based page index (default 1).</param>
        /// <param name="pageSize">Page size (default 10).</param>
        /// <returns>
        /// A <see cref="PaginatedListDto{T}"/> of <see cref="InvitationDto"/> where each item has <see cref="InvitationDto.IsViewedByStudent"/> <c>false</c>.
        /// </returns>
        /// <remarks>
        /// After the student opens an invitation in the UI, call <c>PATCH .../view</c> so it no longer appears in this list.
        /// </remarks>
        /// <response code="200">Paged list of unviewed invitations.</response>
        /// <response code="401">Missing or invalid JWT, or student id cannot be resolved from claims.</response>
        [HttpGet("my/new")]
        [ProducesResponseType(typeof(PaginatedListDto<InvitationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyNewPrivateInvitations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var studentId = _identityService.GetStudentId(User);
            var result = await _invitationService.GetMyNewInvitationsAsync(studentId, pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Legacy alias for <see cref="GetMyPrivateInvitations"/>: returns all private invitations for the authenticated student.
        /// </summary>
        /// <param name="pageNumber">1-based page index (default 1).</param>
        /// <param name="pageSize">Page size (default 10).</param>
        /// <returns>Same as <see cref="GetMyPrivateInvitations"/>.</returns>
        /// <response code="200">Paged list of invitations.</response>
        /// <response code="401">Missing or invalid JWT, or student id cannot be resolved from claims.</response>
        [HttpGet("my")]
        [ProducesResponseType(typeof(PaginatedListDto<InvitationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> GetMyInvitations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
            => GetMyPrivateInvitations(pageNumber, pageSize);

        /// <summary>
        /// Marks a single invitation as viewed by the student (idempotent for already-viewed rows).
        /// </summary>
        /// <param name="id">Invitation primary key (<c>InvitationId</c>).</param>
        /// <returns>No content when the operation completes (invitation missing is ignored at persistence layer).</returns>
        /// <response code="204">Update applied or invitation not found (no error body by design).</response>
        /// <response code="401">Missing or invalid authentication.</response>
        [HttpPatch("{id}/view")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkInvitationViewed(int id)
        {
            await _invitationService.MarkInvitationViewedAsync(id);
            return NoContent();
        }
    }
}
