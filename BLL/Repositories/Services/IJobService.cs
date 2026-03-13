using jobs_service_backend.DTOs.Jobs;
using jobs_service_backend.DTOs.Common;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public interface IJobService
    {
        Task<PaginatedListDto<JobDto>> GetAllPublicJobsAsync(int pageNumber, int pageSize);
        Task<PaginatedListDto<JobDto>> SearchJobsAsync(JobSearchFiltersDto filters);
        Task<JobDto?> GetJobByIdAsync(int id);
        Task<JobDto> CreateJobAsync(CreateJobDto dto);
        Task<bool> UpdateJobAsync(int id, UpdateJobDto dto);
        Task<bool> DeleteJobAsync(int id);
    }
}