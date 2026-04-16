using jobs_service_backend.Clients;
using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs;

public class JobApplicationsListDto
{
    public int ApplicationId { get; set; }

    /// <summary>
    /// Full student profile fetched from the student_profile microservice.
    /// Null if the student profile could not be retrieved.
    /// </summary>
    public StudentProfileDto? Student { get; set; }

    public DateTime AppliedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ApplicationStatus Status { get; set; }

    public string? CoverLetter { get; set; }

    public string? Notes { get; set; }

    public string? ResumeUrl { get; set; }

    public string? JobWebsiteUrl { get; set; }

    public string? JobImageUrl { get; set; }
}

