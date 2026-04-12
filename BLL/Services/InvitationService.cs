using AutoMapper;
using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.DTOs.Invitations;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _repository;
        private readonly IMapper _mapper;

        public InvitationService(IInvitationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task SendInvitationsAsync(int jobId, List<int> studentIds)
        {
            await _repository.SendInvitationsAsync(jobId, studentIds);

            // TODO: send email notifications to invited students
        }

        public async Task<IEnumerable<InvitationDto>> GetMyInvitationsAsync(int studentId)
        {
            var invitations = await _repository.GetMyInvitationsAsync(studentId);
            return _mapper.Map<IEnumerable<InvitationDto>>(invitations);
        }

        public Task<bool> MarkInvitationViewedAsync(int invitationId, int studentId) =>
            _repository.MarkInvitationViewedAsync(invitationId, studentId);
    }
}

