using AutoMapper;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Repositories.Mappings
{
    /// <summary>
    /// AutoMapper profile for <see cref="Application"/> and related DTOs.
    /// </summary>
    /// <remarks>
    /// StudentApplicationsListDto: job title and company from <see cref="Application.Job"/>.
    /// JobApplicationsListDto: <see cref="JobApplicationsListDto.StudentName"/> is ignored until a student source exists.
    /// CreateApplicationDto: identity and timestamps are assigned in the service/repository.
    /// UpdateApplicationStatusDto: maps status and notes only for manager updates.
    /// </remarks>
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Application, StudentApplicationsListDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Job != null ? src.Job.CompanyName : string.Empty));

            CreateMap<Application, JobApplicationsListDto>()
                .ForMember(dest => dest.StudentName, opt => opt.Ignore());

            CreateMap<CreateApplicationDto, Application>()
                .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.AppliedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.Job, opt => opt.Ignore());

            CreateMap<UpdateApplicationStatusDto, Application>()
                .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
                .ForMember(dest => dest.JobId, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.AppliedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CoverLetter, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Job, opt => opt.Ignore());

            CreateMap<Application, UpdateApplicationStatusDto>();
        }
    }
}
