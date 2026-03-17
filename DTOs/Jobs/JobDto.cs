namespace jobs_service_backend.DTOs.Jobs
{
    public class JobDto
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Requirements { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsRemote { get; set; }
        public bool IsPrivate { get; set; }
        public string JobType { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}