using jobs_service_backend.DTOs.Invitations;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public interface IInvitationService
    {
        Task SendInvitationsAsync(int jobId, List<int> studentIds);
        Task<IEnumerable<InvitationDto>> GetMyInvitationsAsync(int studentId);
        Task MarkInvitationViewedAsync(int invitationId);
    }
}

