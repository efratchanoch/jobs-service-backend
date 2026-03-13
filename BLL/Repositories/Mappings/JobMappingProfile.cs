using AutoMapper;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Repositories.Mappings
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            // מיפוי ממשרה ל-DTO (עבור ה-API)
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName).ToList()));

            // מיפוי מ-DTO למשרה (ליצירה/עדכון)
            // אנחנו עושים Ignore ל-Tags כי נטפל בהם ידנית ב-Repository לפי ה-IDs
            CreateMap<CreateJobDto, Job>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<UpdateJobDto, Job>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}