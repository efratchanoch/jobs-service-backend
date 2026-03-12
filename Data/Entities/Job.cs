using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.Data.Entities;

public class Job
{
    public int JobId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public string Experience { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;

    public bool IsRemote { get; set; }
    public bool IsPrivate { get; set; }

    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }

    public bool IsActive { get; set; } = true;

    public JobType JobType { get; set; }
    public Field Field { get; set; }
    public JobStatus Status { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}

