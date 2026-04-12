using AutoMapper;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Invitations;

namespace jobs_service_backend.BLL.Repositories.Mappings
{
    public class InvitationMappingProfile : Profile
    {
        public InvitationMappingProfile()
        {
            CreateMap<PrivateJobInvitation, InvitationDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : string.Empty))
                .ForMember(dest => dest.JobImageUrl, opt => opt.MapFrom(src => src.Job != null ? src.Job.ImageUrl : null))
                .ForMember(dest => dest.JobWebsiteUrl, opt => opt.MapFrom(src => src.Job != null ? src.Job.JobWebsiteUrl : null))
                .ForMember(dest => dest.IsViewedByStudent, opt => opt.MapFrom(src => src.IsViewed));

            CreateMap<BulkInvitationDto, PrivateJobInvitation>()
                .ForMember(dest => dest.InvitationId, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore());
        }
    }
}
