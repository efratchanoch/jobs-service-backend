using jobs_service_backend.Data;
using jobs_service_backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    /// <summary>
    /// Entity Framework implementation of <see cref="IInvitationRepository"/>.
    /// </summary>
    public class InvitationRepository : IInvitationRepository
    {
        private readonly AppDbContext _context;

        public InvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task SendInvitationsAsync(int jobId, List<int> studentIds)
        {
            var invitations = studentIds.Select(studentId => new PrivateJobInvitation
            {
                JobId = jobId,
                StudentId = studentId,
                InvitedAt = DateTime.UtcNow,
                IsViewed = false
            }).ToList();

            await _context.PrivateJobInvitations.AddRangeAsync(invitations);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<PrivateJobInvitation> Invitations, int TotalCount)> GetMyInvitationsAsync(int studentId, int pageNumber, int pageSize)
        {
            var query = _context.PrivateJobInvitations
                .AsNoTracking()
                .Include(i => i.Job)
                .Where(i => i.StudentId == studentId)
                .OrderByDescending(i => i.InvitedAt);

            var totalCount = await query.CountAsync();
            var invitations = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (invitations, totalCount);
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<PrivateJobInvitation> Invitations, int TotalCount)> GetMyNewInvitationsAsync(int studentId, int pageNumber, int pageSize)
        {
            // Closest job deadline first; invitations whose job has no deadline sort last; ties by newest invite.
            var query = _context.PrivateJobInvitations
                .AsNoTracking()
                .Include(i => i.Job)
                .Where(i => i.StudentId == studentId && !i.IsViewed)
                .OrderBy(i => i.Job.Deadline == null ? 1 : 0)
                .ThenBy(i => i.Job.Deadline)
                .ThenByDescending(i => i.InvitedAt);

            var totalCount = await query.CountAsync();
            var invitations = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (invitations, totalCount);
        }

        /// <inheritdoc />
        public async Task MarkInvitationViewedAsync(int invitationId)
        {
            var invitation = await _context.PrivateJobInvitations.FindAsync(invitationId);
            if (invitation != null)
            {
                invitation.IsViewed = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
