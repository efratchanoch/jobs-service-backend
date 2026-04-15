using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs;
using jobs_service_backend.Data.Enums;


namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public interface IApplicationRepository
    {
        Task<(IEnumerable<Application> Applications, int TotalCount)> GetMyApplicationsAsync(int studentId, List<ApplicationStatus>? statuses, bool newestFirst, int pageNumber, int pageSize);
        Task<(IEnumerable<Application> Applications, int TotalCount)> GetApplicationsForJobAsync(int jobId, List<ApplicationStatus>? statuses, bool newestFirst, int pageNumber, int pageSize);
        Task<bool> IsAlreadyAppliedAsync(int studentId, int jobId);
        Task<Application> ApplyToJobAsync(Application application);
        Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto);
        Task<bool> UpdateNotesAsync(int applicationId, string? notes);

        Task<bool> ManagerCanAccessResumeFileAsync(string fileName);
        Task<bool> StudentOwnsResumeFileAsync(string fileName, int studentId);
    }
}
