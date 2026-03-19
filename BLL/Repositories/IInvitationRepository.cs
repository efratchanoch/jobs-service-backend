using jobs_service_backend.Data.Entities;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public interface IInvitationRepository
    {
        Task SendInvitationsAsync(int jobId, List<int> studentIds);
        Task<(IEnumerable<PrivateJobInvitation> Invitations, int TotalCount)> GetMyInvitationsAsync(int studentId, int pageNumber, int pageSize);
        Task MarkInvitationViewedAsync(int invitationId);
    }
}
