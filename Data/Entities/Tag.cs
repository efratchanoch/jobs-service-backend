namespace jobs_service_backend.Data.Entities;

public class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = string.Empty;

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}

