using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs;

public class StudentApplicationDto
{
    public int ApplicationId { get; set; }

    public int JobId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public DateTime AppliedAt { get; set; }

    public ApplicationStatus Status { get; set; }

    public string? CoverLetter { get; set; }
}

