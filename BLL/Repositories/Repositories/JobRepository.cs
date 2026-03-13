using JobsService.Data; 
using JobsService.Data.Entities;
using JobsService.DTOs.Jobs;
using Microsoft.EntityFrameworkCore;

namespace JobsService.BLL.Repositories
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
                .Include(j => j.JobTags)
                    .ThenInclude(jt => jt.Tag) // מביא את השם של התגית
                .Where(j => j.IsActive && !j.IsPrivate); // פעילות ופומביות בלבד

            var totalCount = await query.CountAsync();
            
            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        public async Task<(IEnumerable<Job> Jobs, int TotalCount)> SearchJobsAsync(JobSearchFiltersDto filters)
        {
            var query = _context.Jobs
                .Include(j => j.JobTags)
                    .ThenInclude(jt => jt.Tag)
                .Where(j => j.IsActive && !j.IsPrivate)
                .AsQueryable();

            // סינון דינמי - מוסיפים תנאים רק אם המשתמש שלח אותם
            if (!string.IsNullOrWhiteSpace(filters.FreeText))
            {
                query = query.Where(j => j.Title.Contains(filters.FreeText) || j.Description.Contains(filters.FreeText));
            }

            if (!string.IsNullOrWhiteSpace(filters.Location))
            {
                query = query.Where(j => j.Location.Contains(filters.Location));
            }

            if (filters.Field.HasValue)
            {
                query = query.Where(j => j.Field == filters.Field.Value);
            }

            if (filters.TagIds != null && filters.TagIds.Any())
            {
                // מביא משרות שיש להן לפחות אחת מהתגיות שביקשנו
                query = query.Where(j => j.JobTags.Any(jt => filters.TagIds.Contains(jt.TagId)));
            }

            var totalCount = await query.CountAsync();

            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.JobTags)
                    .ThenInclude(jt => jt.Tag)
                .FirstOrDefaultAsync(j => j.JobId == id && j.IsActive);
        }

        public async Task<Job> CreateJobAsync(Job job, List<int> tagIds)
        {
            // הוספת המשרה למסד
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync(); // שומרים כדי לקבל את ה-JobId שנוצר

            // יצירת קשרי התגיות
            if (tagIds != null && tagIds.Any())
            {
                var jobTags = tagIds.Select(tagId => new JobTag
                {
                    JobId = job.JobId,
                    TagId = tagId
                });
                
                await _context.JobTags.AddRangeAsync(jobTags);
                await _context.SaveChangesAsync();
            }

            return job;
        }

        public async Task UpdateJobAsync(Job job, List<int> tagIds)
        {
            _context.Jobs.Update(job);

            // ניהול תגיות: מוחקים את הישנות ומוסיפים את החדשות (גישה פשוטה ונוחה)
            var existingTags = await _context.JobTags.Where(jt => jt.JobId == job.JobId).ToListAsync();
            _context.JobTags.RemoveRange(existingTags);

            if (tagIds != null && tagIds.Any())
            {
                var newJobTags = tagIds.Select(tagId => new JobTag
                {
                    JobId = job.JobId,
                    TagId = tagId
                });
                await _context.JobTags.AddRangeAsync(newJobTags);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                // מחיקה רכה - אנחנו לא עושים _context.Jobs.Remove(job)
                job.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}