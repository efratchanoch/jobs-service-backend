using AutoMapper;
using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs;
using jobs_service_backend.DTOs.Common;

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

        public async Task<PaginatedListDto<StudentApplicationsListDto>> GetMyApplicationsAsync(int studentId, List<ApplicationStatus>? statuses, bool newestFirst, int pageNumber, int pageSize)
{
    var (applications, totalCount) = await _repository.GetMyApplicationsAsync(studentId, statuses, newestFirst, pageNumber, pageSize);
    var dtos = _mapper.Map<IEnumerable<StudentApplicationsListDto>>(applications);
    return new PaginatedListDto<StudentApplicationsListDto>(dtos, totalCount, pageNumber, pageSize);
}

public async Task<PaginatedListDto<JobApplicationsListDto>> GetApplicationsForJobAsync(int jobId, List<ApplicationStatus>? statuses, bool newestFirst, int pageNumber, int pageSize)
{
    var (applications, totalCount) = await _repository.GetApplicationsForJobAsync(jobId, statuses, newestFirst, pageNumber, pageSize);
    var dtos = _mapper.Map<IEnumerable<JobApplicationsListDto>>(applications);
    return new PaginatedListDto<JobApplicationsListDto>(dtos, totalCount, pageNumber, pageSize);
}



        public async Task<StudentApplicationsListDto> ApplyToJobAsync(CreateApplicationDto dto, int studentId)
        {
            if (await _repository.IsAlreadyAppliedAsync(studentId, dto.JobId))
                throw new InvalidOperationException("You have already applied to this job.");

            var job = await _jobRepository.GetJobByIdAsync(dto.JobId);
            if (job == null || !job.IsActive)
                throw new InvalidOperationException("The job does not exist or is not open for applications.");
            if (job.Deadline.HasValue && job.Deadline.Value < DateTime.UtcNow)
                throw new InvalidOperationException("The application deadline for this job has passed.");

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
