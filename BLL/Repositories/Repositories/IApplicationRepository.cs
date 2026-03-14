using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<Application>> GetMyApplicationsAsync(int studentId);
        Task<IEnumerable<Application>> GetApplicationsForJobAsync(int jobId);
        Task<Application?> ApplyToJobAsync(CreateApplicationDto dto, int studentId);
        Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto);
        Task<bool> UpdateNotesAsync(int applicationId, string? notes);
    }
}
