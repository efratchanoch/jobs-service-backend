namespace jobs_service_backend.DTOs.Invitations
{
    /// <summary>
    /// DTO להצגת נתוני הזמנה למשרה פרטית.
    /// </summary>
    public class InvitationDto
    {
        public int InvitationId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public DateTime InvitedAt { get; set; }
        public bool IsViewedByStudent { get; set; }
    }
}
