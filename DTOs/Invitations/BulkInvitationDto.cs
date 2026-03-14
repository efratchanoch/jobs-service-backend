using System.ComponentModel.DataAnnotations;

namespace jobs_service_backend.DTOs.Invitations
{
    /// <summary>
    /// DTO לשליחת הזמנה מרובה למשרה על ידי המנהלת.
    /// </summary>
    public class BulkInvitationDto
    {
        [Required(ErrorMessage = "Job ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Job ID must be valid.")]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Student list is required.")]
        [MinLength(1, ErrorMessage = "Cannot send invitation to an empty list of students.")]
        public List<int> StudentIds { get; set; } = new();
    }
}
