using jobs_service_backend.Data.Entities;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    /// <summary>
    /// Data access for <see cref="PrivateJobInvitation"/> entities.
    /// </summary>
    public interface IInvitationRepository
    {
        /// <summary>
        /// Inserts one row per student for the given job with <c>InvitedAt = UtcNow</c> and <c>IsViewed = false</c>.
        /// </summary>
        Task SendInvitationsAsync(int jobId, List<int> studentIds);

        /// <summary>
        /// Queries all invitations for a student (any view state), includes navigation to <see cref="PrivateJobInvitation.Job"/>, ordered by invite date descending.
        /// </summary>
        /// <param name="studentId">Foreign key to the student.</param>
        /// <param name="pageNumber">1-based page.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Materialized page plus total row count for pagination.</returns>
        Task<(IEnumerable<PrivateJobInvitation> Invitations, int TotalCount)> GetMyInvitationsAsync(int studentId, int pageNumber, int pageSize);

        /// <summary>
        /// Unviewed invitations only (<c>IsViewed == false</c>), ordered by the related job's <see cref="Job.Deadline"/> ascending (soonest first);
        /// jobs without a deadline appear after those with a deadline; ties break by <see cref="PrivateJobInvitation.InvitedAt"/> descending.
        /// </summary>
        Task<(IEnumerable<PrivateJobInvitation> Invitations, int TotalCount)> GetMyNewInvitationsAsync(int studentId, int pageNumber, int pageSize);

        /// <summary>
        /// Loads the invitation by id for the given student and sets <c>IsViewed</c> to true when found.
        /// </summary>
        Task<bool> MarkInvitationViewedAsync(int invitationId, int studentId);
    }
}
