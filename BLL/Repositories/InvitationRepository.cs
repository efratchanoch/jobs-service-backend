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

        public async Task<IEnumerable<PrivateJobInvitation>> GetMyInvitationsAsync(int studentId)
        {
            return await _context.PrivateJobInvitations
                .AsNoTracking()
                .Include(i => i.Job)
                .Where(i => i.StudentId == studentId)
                .OrderByDescending(i => i.InvitedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkInvitationViewedAsync(int invitationId, int studentId)
        {
            var invitation = await _context.PrivateJobInvitations
                .FirstOrDefaultAsync(i => i.InvitationId == invitationId && i.StudentId == studentId);
            if (invitation == null)
                return false;

            invitation.IsViewed = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
