using System.Security.Claims;
using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.DTOs.Invitations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jobs_service_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationsController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        // 1. שליחת הזמנה מרובה – מנהלת בלבד
        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendBulkInvitations([FromBody] BulkInvitationDto dto)
        {
            await _invitationService.SendInvitationsAsync(dto.JobId, dto.StudentIds);

            // TODO: handle automatic email notifications to invited students
            return NoContent();
        }

        // 2. שליפת ההזמנות שלי – לפי סטודנטית מתוך ה-JWT
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyInvitations()
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("sub")?.Value;

            if (!int.TryParse(studentIdClaim, out var studentId))
            {
                return Unauthorized("Student ID is missing or invalid in the token.");
            }

            var invitations = await _invitationService.GetMyInvitationsAsync(studentId);
            return Ok(invitations);
        }

        // 3. סימון הזמנה כנצפתה
        [HttpPatch("{id}/view")]
        [Authorize]
        public async Task<IActionResult> MarkInvitationViewed(int id)
        {
            await _invitationService.MarkInvitationViewedAsync(id);
            return NoContent();
        }
    }
}

