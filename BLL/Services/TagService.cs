using AutoMapper;
using jobs_service_backend.BLL.Repositories.Repositories;
using jobs_service_backend.Data.Entities;
using jobs_service_backend.DTOs.Tags;

namespace jobs_service_backend.BLL.Repositories.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _repository.GetAllTagsAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto> CreateTagAsync(CreateTagDto dto)
        {
            var name = dto.TagName.Trim();
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Tag name cannot be empty.");

            var existing = await _repository.GetAllTagsAsync();
            if (existing.Any(t => string.Equals(t.TagName, name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A tag with this name already exists.");

            var tag = new Tag { TagName = name };
            var created = await _repository.AddTagAsync(tag);
            return _mapper.Map<TagDto>(created);
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            return await _repository.DeleteTagAsync(tagId);
        }
    }
}
