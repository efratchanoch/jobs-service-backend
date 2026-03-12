using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs;

public class JobApplicationDto
{
    public int ApplicationId { get; set; }

    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public DateTime AppliedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ApplicationStatus Status { get; set; }

    public string? CoverLetter { get; set; }

    public string? Notes { get; set; }
}

