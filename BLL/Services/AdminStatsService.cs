using jobs_service_backend.Data;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs.Admin;
using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.BLL.Repositories.Services;

public class AdminStatsService : IAdminStatsService
{
    private readonly AppDbContext _dbContext;

    public AdminStatsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AdminStatsDto> GetDashboardStatsAsync(CancellationToken cancellationToken = default)
    {
        var jobsQuery = _dbContext.Jobs
            .AsNoTracking()
            .Where(j => j.IsActive && !j.IsPrivate);

        var jobStatusCounts = await jobsQuery
            .GroupBy(j => j.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);

        var totalJobs = jobStatusCounts.Sum(item => item.Count);
        var openJobs = jobStatusCounts.FirstOrDefault(item => item.Status == JobStatus.Open)?.Count ?? 0;
        var pendingJobs = jobStatusCounts.FirstOrDefault(item => item.Status == JobStatus.Pending)?.Count ?? 0;
        var closedJobs = jobStatusCounts.FirstOrDefault(item => item.Status == JobStatus.Closed)?.Count ?? 0;

        var placementsCompleted = await _dbContext.Applications
            .AsNoTracking()
            .CountAsync(a => a.Job.IsActive && !a.Job.IsPrivate && a.Status == ApplicationStatus.Accepted, cancellationToken);

        return new AdminStatsDto
        {
            TotalJobs = totalJobs,
            OpenJobs = openJobs,
            PendingJobs = pendingJobs,
            ClosedJobs = closedJobs,
            PlacementsCompleted = placementsCompleted
        };
    }
}
