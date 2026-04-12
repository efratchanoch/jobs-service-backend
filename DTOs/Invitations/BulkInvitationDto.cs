using System.ComponentModel.DataAnnotations;

namespace jobs_service_backend.DTOs.Invitations
{
    /// <summary>
    /// Request body for sending private invitations to multiple students for one job (manager operation).
    /// </summary>
    public class BulkInvitationDto
    {
        /// <summary>Target job id.</summary>
        [Required(ErrorMessage = "Job ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Job ID must be valid.")]
        public int JobId { get; set; }

        /// <summary>Distinct or duplicate student ids to invite; must contain at least one id.</summary>
        [Required(ErrorMessage = "Student list is required.")]
        [MinLength(1, ErrorMessage = "Cannot send invitation to an empty list of students.")]
        public List<int> StudentIds { get; set; } = new();
    }
}
