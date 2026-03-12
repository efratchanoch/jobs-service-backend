using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs;

public class UpdateApplicationStatusDto
{
    public int ApplicationId { get; set; }

    public ApplicationStatus Status { get; set; }

    public string? Notes { get; set; }
}

