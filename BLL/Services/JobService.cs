using AutoMapper;
using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.BLL.Repositories.Services;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.Data.Enums;
using jobs_service_backend.DTOs.Jobs;
using jobs_service_backend.DTOs.Common;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repository;
        private readonly IMapper _mapper;

        public JobService(IJobRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
public async Task<PaginatedListDto<JobDto>> GetAllPublicJobsAsync(List<JobStatus>? statuses, bool newestFirst, int pageNumber, int pageSize)
{
    var (pn, ps) = Pagination.Normalize(pageNumber, pageSize);
    var (jobs, totalCount) = await _repository.GetAllPublicJobsAsync(statuses, newestFirst, pn, ps);
    var dtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
    return new PaginatedListDto<JobDto>(dtos, totalCount, pn, ps);
}

public async Task<PaginatedListDto<JobDto>> SearchJobsAsync(JobSearchFiltersDto filters)
{
    var (pn, ps) = Pagination.Normalize(filters.PageNumber, filters.PageSize);
    var (jobs, totalCount) = await _repository.SearchJobsAsync(filters, pn, ps);
    var dtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
    return new PaginatedListDto<JobDto>(dtos, totalCount, pn, ps);
}

        public async Task<JobDto?> GetJobByIdAsync(int id)
        {
            var job = await _repository.GetJobByIdAsync(id);
            return _mapper.Map<JobDto>(job);
        }

        public async Task<JobDto> CreateJobAsync(CreateJobDto dto)
        {
            var jobEntity = _mapper.Map<Job>(dto);
            var createdJob = await _repository.CreateJobAsync(jobEntity, dto.TagIds);
            return _mapper.Map<JobDto>(createdJob);
        }

public async Task<bool> UpdateJobAsync(int id, UpdateJobDto dto)
{
    var existingJob = await _repository.GetJobByIdAsync(id);
    if (existingJob == null) return false;

    _mapper.Map(dto, existingJob);
    
    await _repository.UpdateJobAsync(existingJob, dto.TagIds);
    return true;
}
        public async Task<bool> DeleteJobAsync(int id)
        {
            var success = await _repository.GetJobByIdAsync(id);
            if (success == null) return false;
            await _repository.DeleteJobAsync(id);
            return true;
        }
    }
}