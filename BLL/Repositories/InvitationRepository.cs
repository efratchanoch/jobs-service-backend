using jobs_service_backend.Data;
using jobs_service_backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly AppDbContext _context;

        public InvitationRepository(AppDbContext context)
        {
            _context = context;
        }

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
