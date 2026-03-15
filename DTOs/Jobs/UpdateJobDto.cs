using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.DTOs.Jobs
{
    public class UpdateJobDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Requirements { get; set; } = string.Empty;
        public JobStatus Status { get; set; }
        public DateTime? Deadline { get; set; }
        public List<int> TagIds { get; set; } = new();
    }
}