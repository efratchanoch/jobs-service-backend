using AutoMapper;
using JobsService.Data.Entities; // הנתיב למודלים של בחורה 1
using JobsService.DTOs.Jobs;

namespace JobsService.BLL.Mappings
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            // מיפוי מהמודל ל-DTO: הופכים את התגיות מרשימת אובייקטים לרשימת מחרוזות יפה
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.JobTags.Select(jt => jt.Tag.TagName).ToList()))
                .ForMember(dest => dest.JobType, opt => opt.MapFrom(src => src.JobType.ToString()))
                .ForMember(dest => dest.Field, opt => opt.MapFrom(src => src.Field.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // מיפוי מיצירת משרה למודל: מגדירים ערכי ברירת מחדל
            CreateMap<CreateJobDto, Job>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Data.Enums.JobStatus.Open))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.JobTags, opt => opt.Ignore()); // את קשרי התגיות ננהל ידנית ב-Service

            // מיפוי מעדכון משרה
            CreateMap<UpdateJobDto, Job>()
                .ForMember(dest => dest.JobTags, opt => opt.Ignore());
        }
    }
}