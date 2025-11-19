using AutoMapper;
using Blogy.Business.DTOs.TagDtos;
using Blogy.DataAccess.Repositories.TagRepositories;

namespace Blogy.Business.Services.TagServices
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultTagDto>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return _mapper.Map<List<ResultTagDto>>(tags);
        }

        public async Task<List<ResultTagDto>> GetAllTagsWithBlogCountAsync()
        {
            var tags = await _tagRepository.GetAllWithBlogCountAsync();
            return _mapper.Map<List<ResultTagDto>>(tags);
        }
    }
}
