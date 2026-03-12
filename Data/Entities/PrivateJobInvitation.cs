namespace jobs_service_backend.Data.Entities;

public class PrivateJobInvitation
{
    public int InvitationId { get; set; }

    public int JobId { get; set; }
    public int StudentId { get; set; }

    public DateTime InvitedAt { get; set; } = DateTime.UtcNow;
    public bool IsViewed { get; set; } = false;

    public Job Job { get; set; } = null!;
}

