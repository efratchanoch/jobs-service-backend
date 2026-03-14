using AutoMapper;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Repositories.Mappings
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            // Application -> StudentApplicationsListDto (להצגת מועמדויות לתלמידה)
            CreateMap<Application, StudentApplicationsListDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Job != null ? src.Job.CompanyName : string.Empty));

            // Application -> JobApplicationsListDto (להצגת מועמדויות למשרה למנהלת)
            CreateMap<Application, JobApplicationsListDto>()
                .ForMember(dest => dest.StudentName, opt => opt.Ignore()); // אין ישות Student – למלא ממקור חיצוני אם נדרש

            // CreateApplicationDto -> Application (ליצירת הגשה – שאר השדות נקבעים ב-Repository)
            CreateMap<CreateApplicationDto, Application>()
                .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.AppliedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.Job, opt => opt.Ignore());

            // UpdateApplicationStatusDto -> Application (לעדכון סטטוס והערות – רק Status ו-Notes)
            CreateMap<UpdateApplicationStatusDto, Application>()
                .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
                .ForMember(dest => dest.JobId, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.AppliedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CoverLetter, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Job, opt => opt.Ignore());

            // Application -> UpdateApplicationStatusDto (לשליחה/תצוגה של סטטוס נוכחי)
            CreateMap<Application, UpdateApplicationStatusDto>();
        }
    }
}
