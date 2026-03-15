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

        // 1. שליפת כל המשרות הציבוריות והפעילות
        public async Task<(IEnumerable<Job> Jobs, int TotalCount)> GetAllPublicJobsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Jobs
                .AsNoTracking()
                .Include(j => j.Tags)
                .Where(j => !j.IsPrivate && j.IsActive);

            var totalCount = await query.CountAsync();
            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        // 2. חיפוש וסינון מתקדם (כולל תגיות)
        public async Task<(IEnumerable<Job> Jobs, int TotalCount)> SearchJobsAsync(JobSearchFiltersDto filters)
        {
            var query = _context.Jobs
                .AsNoTracking()
                .Include(j => j.Tags)
                .Where(j => j.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filters.FreeText))
            {
                query = query.Where(j => j.Title.Contains(filters.FreeText) 
                                      || j.CompanyName.Contains(filters.FreeText)
                                      || j.Description.Contains(filters.FreeText));
            }

            if (!string.IsNullOrEmpty(filters.Location))
                query = query.Where(j => j.Location.Contains(filters.Location));

            if (filters.Field.HasValue)
                query = query.Where(j => j.Field == filters.Field.Value);

            if (filters.TagIds != null && filters.TagIds.Any())
                query = query.Where(j => j.Tags.Any(t => filters.TagIds.Contains(t.TagId)));

            var totalCount = await query.CountAsync();
            var jobs = await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        // 3. שליפת משרה בודדת
        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.Tags)
                .FirstOrDefaultAsync(j => j.JobId == id && j.IsActive);
        }

        // 4. יצירת משרה חדשה עם תגיות
        public async Task<Job> CreateJobAsync(Job job, List<int> tagIds)
        {
            if (tagIds != null && tagIds.Any())
            {
                var tags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
                job.Tags = tags;
            }

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        // 5. עדכון משרה (כולל ניקוי ועדכון תגיות מחדש)
        public async Task UpdateJobAsync(Job job, List<int> tagIds)
        {
            var existingJob = await _context.Jobs
                .Include(j => j.Tags)
                .FirstOrDefaultAsync(j => j.JobId == job.JobId);

            if (existingJob != null)
            {
                // עדכון שדות פשוטים
                _context.Entry(existingJob).CurrentValues.SetValues(job);
                
                // עדכון תגיות: מוחקים את הישנות ומביאים את החדשות מה-DB
                existingJob.Tags.Clear();
                if (tagIds != null && tagIds.Any())
                {
                    var tags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
                    foreach (var tag in tags) existingJob.Tags.Add(tag);
                }

                await _context.SaveChangesAsync();
            }
        }

        // 6. מחיקה רכה (Soft Delete)
        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.IsActive = false; // לא באמת מוחקים! רק מכבים
                await _context.SaveChangesAsync();
            }
        }
    }
}