using JobsService.Data.Entities; 
using JobsService.DTOs.Jobs;

namespace JobsService.BLL.Repositories
{
    public interface IJobRepository
    {
        // מחזיר גם את הרשימה וגם את הכמות הכוללת בשביל פאג'ינציה (Pagination)
        Task<(IEnumerable<Job> Jobs, int TotalCount)> GetAllPublicJobsAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Job> Jobs, int TotalCount)> SearchJobsAsync(JobSearchFiltersDto filters);
        Task<Job?> GetJobByIdAsync(int id);
        
        // יצירה, עדכון ומחיקה
        Task<Job> CreateJobAsync(Job job, List<int> tagIds);
        Task UpdateJobAsync(Job job, List<int> tagIds);
        Task DeleteJobAsync(int id); // מחיקה רכה (Soft Delete)
    }
}