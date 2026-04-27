namespace jobs_service_backend.DTOs.Admin;

public class AdminStatsDto
{
    public int TotalJobs { get; set; }
    public int OpenJobs { get; set; }
    public int PendingJobs { get; set; }
    public int ClosedJobs { get; set; }
    public int PlacementsCompleted { get; set; }
}
