using jobs_service_backend.Data;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Jobs;
using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Job> Jobs, int TotalCount)> GetAllPublicJobsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Jobs
                .Include(j => j.Tags) // שימוש ב-Tags הישיר
                .Where(j => !j.IsPrivate && j.IsActive);

            var totalCount = await query.CountAsync();
            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.Tags)
                .FirstOrDefaultAsync(j => j.JobId == id);
        }

        public async Task<Job> CreateJobAsync(Job job, List<int> tagIds)
        {
            if (tagIds != null && tagIds.Any())
            {
                var tags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
                job.Tags = tags; // הוספת הטגיות ישירות לאוסף
            }

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public async Task UpdateJobAsync(Job job, List<int> tagIds)
        {
            // טעינת המשרה עם הטגיות הקיימות
            var existingJob = await _context.Jobs
                .Include(j => j.Tags)
                .FirstOrDefaultAsync(j => j.JobId == job.JobId);

            if (existingJob != null)
            {
                _context.Entry(existingJob).CurrentValues.SetValues(job);
                
                // עדכון הטגיות בקשר Many-to-Many
                existingJob.Tags.Clear();
                if (tagIds != null && tagIds.Any())
                {
                    var tags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
                    foreach (var tag in tags) existingJob.Tags.Add(tag);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

public async Task<(IEnumerable<Job> Jobs, int TotalCount)> SearchJobsAsync(JobSearchFiltersDto filters)
{
    var query = _context.Jobs.Include(j => j.Tags).AsQueryable();

    // שימוש ב-FreeText כפי שהגדרת ב-DTO
    if (!string.IsNullOrEmpty(filters.FreeText))
    {
        query = query.Where(j => j.Title.Contains(filters.FreeText) 
                              || j.CompanyName.Contains(filters.FreeText)
                              || j.Description.Contains(filters.FreeText));
    }

    // סינון לפי מיקום (ראיתי שיש לך ב-DTO)
    if (!string.IsNullOrEmpty(filters.Location))
    {
        query = query.Where(j => j.Location.Contains(filters.Location));
    }

    if (filters.Field.HasValue)
    {
        query = query.Where(j => j.Field == filters.Field.Value);
    }

    var totalCount = await query.CountAsync();
    var jobs = await query
        .Skip((filters.PageNumber - 1) * filters.PageSize)
        .Take(filters.PageSize)
        .ToListAsync();

    return (jobs, totalCount);
}
    }
}