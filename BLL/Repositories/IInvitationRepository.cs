using jobs_service_backend.Data.Entities;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public interface IInvitationRepository
    {
        Task SendInvitationsAsync(int jobId, List<int> studentIds);
        Task<IEnumerable<PrivateJobInvitation>> GetMyInvitationsAsync(int studentId);
        Task<bool> MarkInvitationViewedAsync(int invitationId, int studentId);
    }
}
