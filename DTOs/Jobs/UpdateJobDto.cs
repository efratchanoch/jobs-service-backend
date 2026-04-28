using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs.Jobs
{
    /// <summary>Full job update (aligned with <see cref="Data.Entities.Job"/> and create flow).</summary>
    public class UpdateJobDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Requirements { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public string? JobWebsiteUrl { get; set; }
        public string? JobImageUrl { get; set; }

        public bool IsRemote { get; set; }
        public bool IsPrivate { get; set; }

        public JobType JobType { get; set; }
        public Field Field { get; set; }

        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }

        public JobStatus Status { get; set; }
        public DateTime? Deadline { get; set; }
        public List<int> TagIds { get; set; } = new();
    }
}