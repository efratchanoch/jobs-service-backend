using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Invitations;
using jobs_service_backend.DTOs.Common;

namespace jobs_service_backend.BLL.Repositories.Services
{
    /// <summary>
    /// Application service for private job invitations (orchestration and DTO mapping).
    /// </summary>
    public interface IInvitationService
    {
        /// <summary>
        /// Persists invitations for the given job and student identifiers.
        /// </summary>
        /// <param name="jobId">Private (or any) job identifier.</param>
        /// <param name="studentIds">Students to invite; duplicates are not deduplicated here.</param>
        Task SendInvitationsAsync(int jobId, List<int> studentIds);

        /// <summary>
        /// Loads every invitation for the student, ordered by <see cref="PrivateJobInvitation.InvitedAt"/> descending.
        /// </summary>
        /// <param name="studentId">Resolved from the authenticated user.</param>
        /// <param name="pageNumber">1-based page.</param>
        /// <param name="pageSize">Items per page.</param>
        /// <returns>Paginated <see cref="InvitationDto"/> collection.</returns>
        Task<PaginatedListDto<InvitationDto>> GetMyInvitationsAsync(int studentId, int pageNumber, int pageSize);

        /// <summary>
        /// Loads unviewed invitations (<c>IsViewed == false</c>), sorted by related job deadline (soonest first; no deadline last), then by invite date.
        /// </summary>
        /// <param name="studentId">Resolved from the authenticated user.</param>
        /// <param name="pageNumber">1-based page.</param>
        /// <param name="pageSize">Items per page.</param>
        /// <returns>Paginated <see cref="InvitationDto"/> collection; each item should have <see cref="InvitationDto.IsViewedByStudent"/> false.</returns>
        Task<PaginatedListDto<InvitationDto>> GetMyNewInvitationsAsync(int studentId, int pageNumber, int pageSize);

        /// <summary>
        /// Sets <c>IsViewed</c> to true for the invitation when it exists and belongs to the student.
        /// </summary>
        Task<bool> MarkInvitationViewedAsync(int invitationId, int studentId);
    }
}
