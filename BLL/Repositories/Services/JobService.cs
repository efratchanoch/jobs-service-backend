using AutoMapper;
using JobsService.BLL.Repositories;
using JobsService.Data.Entities;
using JobsService.DTOs.Jobs;

namespace JobsService.BLL.Services
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

        public async Task<PaginatedListDto<JobDto>> GetAllPublicJobsAsync(int pageNumber, int pageSize)
        {
            var (jobs, totalCount) = await _repository.GetAllPublicJobsAsync(pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
            
            return new PaginatedListDto<JobDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginatedListDto<JobDto>> SearchJobsAsync(JobSearchFiltersDto filters)
        {
            var (jobs, totalCount) = await _repository.SearchJobsAsync(filters);
            var dtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
            
            return new PaginatedListDto<JobDto>(dtos, totalCount, filters.PageNumber, filters.PageSize);
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

            _mapper.Map(dto, existingJob); // מעדכן את האובייקט הקיים בערכים החדשים
            await _repository.UpdateJobAsync(existingJob, dto.TagIds);
            return true;
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _repository.GetJobByIdAsync(id);
            if (job == null) return false;

            await _repository.DeleteJobAsync(id);
            return true;
        }

        public async Task<int> CloseExpiredJobsAsync()
        {
            // כאן תבוא לוגיקה שסוגרת משרות שה-Deadline שלהן עבר
            // זה "הגדלת ראש" - אנחנו מחפשים את כל המשרות הפתוחות שהתאריך שלהן עבר
            // וקוראים ל-Repository לעדכן אותן ל-Closed
            return 0; // נחזיר את מספר המשרות שנסגרו
        }
    }
}