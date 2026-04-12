using AutoMapper;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Tags;

namespace jobs_service_backend.BLL.Repositories.Mappings
{
    public class TagMappingProfile : Profile
    {
        public TagMappingProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}
