using AutoMapper;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Repositories.Mappings
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.JobType, opt => opt.MapFrom(src => src.JobType.ToString()))
                .ForMember(dest => dest.Field, opt => opt.MapFrom(src => src.Field.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName).ToList()));

            CreateMap<CreateJobDto, Job>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<UpdateJobDto, Job>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}