using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.BLL.Services;
using jobs_service_backend.DTOs.Invitations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
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

        // 1. שליחת הזמנה מרובה – מנהלת בלבד
        [HttpPost("bulk")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SendBulkInvitations([FromBody] BulkInvitationDto dto)
        {
            await _invitationService.SendInvitationsAsync(dto.JobId, dto.StudentIds);

            // TODO: handle automatic email notifications to invited students
            return NoContent();
        }

        // 2. שליפת ההזמנות שלי – לפי סטודנטית מתוך ה-JWT
        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyInvitations()
        {
            var studentId = _identityService.GetStudentId(User);
            var invitations = await _invitationService.GetMyInvitationsAsync(studentId);
            return Ok(invitations);
        }

        // 3. סימון הזמנה כנצפתה
        [HttpPatch("{id}/view")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MarkInvitationViewed(int id)
        {
            var studentId = _identityService.GetStudentId(User);
            var updated = await _invitationService.MarkInvitationViewedAsync(id, studentId);
            if (!updated)
                return NotFound();
            return NoContent();
        }
    }
}

