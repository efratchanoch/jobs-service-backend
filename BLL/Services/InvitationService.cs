using AutoMapper;
using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.DTOs.Common;
using jobs_service_backend.DTOs.Invitations;

namespace jobs_service_backend.BLL.Repositories.Services
{
    /// <summary>
    /// Default implementation of <see cref="IInvitationService"/> using EF repositories and AutoMapper.
    /// </summary>
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _repository;
        private readonly IMapper _mapper;

        public InvitationService(IInvitationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task SendInvitationsAsync(int jobId, List<int> studentIds)
        {
            await _repository.SendInvitationsAsync(jobId, studentIds);

            // TODO: send email notifications to invited students
        }

        /// <inheritdoc />
        public async Task<PaginatedListDto<InvitationDto>> GetMyInvitationsAsync(int studentId, int pageNumber, int pageSize)
        {
            var (invitations, totalCount) = await _repository.GetMyInvitationsAsync(studentId, pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<InvitationDto>>(invitations);
            return new PaginatedListDto<InvitationDto>(dtos, totalCount, pageNumber, pageSize);
        }

        /// <inheritdoc />
        public async Task<PaginatedListDto<InvitationDto>> GetMyNewInvitationsAsync(int studentId, int pageNumber, int pageSize)
        {
            var (invitations, totalCount) = await _repository.GetMyNewInvitationsAsync(studentId, pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<InvitationDto>>(invitations);
            return new PaginatedListDto<InvitationDto>(dtos, totalCount, pageNumber, pageSize);
        }

        /// <inheritdoc />
        public async Task MarkInvitationViewedAsync(int invitationId)
        {
            await _repository.MarkInvitationViewedAsync(invitationId);
        }
    }
}
