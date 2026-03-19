using jobs_service_backend.DTOs.Invitations;
using jobs_service_backend.DTOs.Common;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public interface IInvitationService
    {
        Task SendInvitationsAsync(int jobId, List<int> studentIds);
        Task<PaginatedListDto<InvitationDto>> GetMyInvitationsAsync(int studentId, int pageNumber, int pageSize);
        Task MarkInvitationViewedAsync(int invitationId);
    }
}

