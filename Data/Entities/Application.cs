using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.Data.Entities;

public class Application
{
    public int ApplicationId { get; set; }

    public int JobId { get; set; }
    public int StudentId { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ApplicationStatus Status { get; set; }

    public string? CoverLetter { get; set; }
    public string? Notes { get; set; }

    public Job Job { get; set; } = null!;
}

