using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Jobs;
using jobs_service_backend.Data.Enums;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public interface IJobRepository
    {
        Task<(IEnumerable<Job> Jobs, int TotalCount)> GetAllPublicJobsAsync(List<JobStatus>? statuses, bool newestFirst, int pageNumber, int pageSize);
        Task<(IEnumerable<Job> Jobs, int TotalCount)> SearchJobsAsync(JobSearchFiltersDto filters);
        Task<Job?> GetJobByIdAsync(int id);
        Task<Job> CreateJobAsync(Job job, List<int> tagIds);
        Task UpdateJobAsync(Job job, List<int> tagIds);
        Task DeleteJobAsync(int id);
    }
}