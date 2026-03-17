using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<StudentApplicationsListDto>> GetMyApplicationsAsync(int studentId);
        Task<IEnumerable<JobApplicationsListDto>> GetApplicationsForJobAsync(int jobId);
        Task<StudentApplicationsListDto> ApplyToJobAsync(CreateApplicationDto dto, int studentId);
        Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto);
        Task<bool> UpdateNotesAsync(int applicationId, string? notes);
    }
}
