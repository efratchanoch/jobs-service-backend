using jobs_service_backend.DTOs.Admin;

namespace jobs_service_backend.BLL.Repositories.Services;

public interface IAdminStatsService
{
    Task<AdminStatsDto> GetDashboardStatsAsync(CancellationToken cancellationToken = default);
}
