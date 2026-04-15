using jobs_service_backend.Data;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAlreadyAppliedAsync(int studentId, int jobId)
        {
            return await _context.Applications
                .AnyAsync(a => a.StudentId == studentId && a.JobId == jobId);
        }

        public async Task<(IEnumerable<Application> Applications, int TotalCount)> GetMyApplicationsAsync(int studentId, List<ApplicationStatus>? statuses, bool newestFirst, int pageNumber, int pageSize)
{
    var query = _context.Applications
        .AsNoTracking()
        .Include(a => a.Job)
        .Where(a => a.StudentId == studentId);

    if (statuses != null && statuses.Count > 0)
        query = query.Where(a => statuses.Contains(a.Status));

    query = newestFirst
        ? query.OrderByDescending(a => a.AppliedAt)
        : query.OrderBy(a => a.AppliedAt);

    var totalCount = await query.CountAsync();
    var applications = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (applications, totalCount);
}

public async Task<(IEnumerable<Application> Applications, int TotalCount)> GetApplicationsForJobAsync(int jobId, List<ApplicationStatus>? statuses, bool newestFirst, int pageNumber, int pageSize)
{
    var query = _context.Applications
        .AsNoTracking()
        .Include(a => a.Job)
        .Where(a => a.JobId == jobId);

    if (statuses != null && statuses.Count > 0)
        query = query.Where(a => statuses.Contains(a.Status));

    query = newestFirst
        ? query.OrderByDescending(a => a.UpdatedAt ?? a.AppliedAt)
        : query.OrderBy(a => a.UpdatedAt ?? a.AppliedAt);

    var totalCount = await query.CountAsync();
    var applications = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (applications, totalCount);
}


        public async Task<Application> ApplyToJobAsync(Application application)
        {
            application.AppliedAt = DateTime.UtcNow;
            application.Status = ApplicationStatus.Pending;
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            await _context.Entry(application).Reference(a => a.Job).LoadAsync();
            return application;
        }

        public async Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto)
        {
            var application = await _context.Applications.FindAsync(dto.ApplicationId);
            if (application == null)
                return false;

            application.Status = dto.Status;
            application.Notes = dto.Notes;
            application.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNotesAsync(int applicationId, string? notes)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null)
                return false;

            application.Notes = notes;
            application.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ManagerCanAccessResumeFileAsync(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(fileName)) return false;

            return await _context.Applications
                .AsNoTracking()
                .AnyAsync(a => a.ResumeUrl != null && a.ResumeUrl.EndsWith("/" + fileName));
        }

        public async Task<bool> StudentOwnsResumeFileAsync(string fileName, int studentId)
        {
            fileName = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(fileName)) return false;

            return await _context.Applications
                .AsNoTracking()
                .AnyAsync(a =>
                    a.StudentId == studentId
                    && a.ResumeUrl != null
                    && a.ResumeUrl.EndsWith("/" + fileName));
        }
    }
}
