using AutoMapper;
using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository repository, IJobRepository jobRepository, IMapper mapper)
        {
            _repository = repository;
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentApplicationsListDto>> GetMyApplicationsAsync(int studentId)
        {
            var applications = await _repository.GetMyApplicationsAsync(studentId);
            return _mapper.Map<IEnumerable<StudentApplicationsListDto>>(applications);
        }

        public async Task<IEnumerable<JobApplicationsListDto>> GetApplicationsForJobAsync(int jobId)
        {
            var applications = await _repository.GetApplicationsForJobAsync(jobId);
            return _mapper.Map<IEnumerable<JobApplicationsListDto>>(applications);
        }

        public async Task<StudentApplicationsListDto> ApplyToJobAsync(CreateApplicationDto dto, int studentId)
        {
            if (await _repository.IsAlreadyAppliedAsync(studentId, dto.JobId))
                throw new InvalidOperationException("התלמידה כבר הגישה מועמדות למשרה זו.");

            var job = await _jobRepository.GetJobByIdAsync(dto.JobId);
            if (job == null || !job.IsActive)
                throw new InvalidOperationException("המשרה אינה קיימת או אינה פתוחה להגשה.");
            if (job.Deadline.HasValue && job.Deadline.Value < DateTime.UtcNow)
                throw new InvalidOperationException("תאריך הסגירה להגשת מועמדות למשרה זו עבר.");

            var application = _mapper.Map<Application>(dto);
            application.StudentId = studentId;

            var created = await _repository.ApplyToJobAsync(application);
            return _mapper.Map<StudentApplicationsListDto>(created);
        }

        public async Task<bool> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto)
        {
            return await _repository.UpdateApplicationStatusAsync(dto);
        }

        public async Task<bool> UpdateNotesAsync(int applicationId, string? notes)
        {
            return await _repository.UpdateNotesAsync(applicationId, notes);
        }
    }
}
