using jobs_service_backend.DTOs;
using jobs_service_backend.DTOs.Common;
using jobs_service_backend.Data.Enums;




namespace jobs_service_backend.BLL.Repositories.Services
{
    public interface IApplicationService
    {
        Task<PaginatedListDto<StudentApplicationsListDto>> GetMyApplicationsAsync(int studentId, List<ApplicationStatus>? statuses, int pageNumber, int pageSize); 
        Task<PaginatedListDto<JobApplicationsListDto>> GetApplicationsForJobAsync(int jobId, List<ApplicationStatus>? statuses, int pageNumber, int pageSize); 
        Task<StudentApplicationsListDto> ApplyToJobAsync(CreateApplicationDto dto, int studentId);
        Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto);
        Task<bool> UpdateNotesAsync(int applicationId, string? notes);
    }
}
