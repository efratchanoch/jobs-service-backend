using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs;

public class StudentApplicationsListDto
{
    public int ApplicationId { get; set; }

    public int JobId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string? JobWebsiteUrl { get; set; }

    public string? JobImageUrl { get; set; }

    public DateTime AppliedAt { get; set; }

    public ApplicationStatus Status { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CoverLetter { get; set; }

    public string? ResumeUrl { get; set; }
}

