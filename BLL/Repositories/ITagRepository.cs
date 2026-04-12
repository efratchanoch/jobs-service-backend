using jobs_service_backend.Data.Entities;

namespace jobs_service_backend.BLL.Repositories.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> AddTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(int tagId);
    }
}
