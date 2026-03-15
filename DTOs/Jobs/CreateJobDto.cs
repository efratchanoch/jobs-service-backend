using jobs_service_backend.Data.Enums; 

namespace jobs_service_backend.DTOs.Jobs
{
    public class CreateJobDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Requirements { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsRemote { get; set; }
        public bool IsPrivate { get; set; }
        public JobType JobType { get; set; }
        public Field Field { get; set; }
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public DateTime Deadline { get; set; }
        
        // אנחנו מבקשים מהקליינט לשלוח רק את ה-IDs של התגיות שהוא רוצה לקשר
        public List<int> TagIds { get; set; } = new(); 
    }
}