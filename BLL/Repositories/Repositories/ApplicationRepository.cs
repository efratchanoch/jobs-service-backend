using jobs_service_backend.Data;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Application>> GetMyApplicationsAsync(int studentId)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Job)
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsForJobAsync(int jobId)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.UpdatedAt ?? a.AppliedAt)
                .ToListAsync();
        }

        public async Task<Application?> ApplyToJobAsync(CreateApplicationDto dto, int studentId)
        {
            var alreadyApplied = await _context.Applications
                .AnyAsync(a => a.JobId == dto.JobId && a.StudentId == studentId);
            if (alreadyApplied)
                return null;

            var application = new Application
            {
                JobId = dto.JobId,
                StudentId = studentId,
                CoverLetter = dto.CoverLetter,
                Status = ApplicationStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
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
    }
}
