using jobs_service_backend.DTOs.Tags;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> CreateTagAsync(CreateTagDto dto);
        Task<bool> DeleteTagAsync(int tagId);
    }
}
