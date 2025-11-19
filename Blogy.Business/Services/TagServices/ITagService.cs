using Blogy.Business.DTOs.TagDtos;

namespace Blogy.Business.Services.TagServices
{
    public interface ITagService
    {
        Task<List<ResultTagDto>> GetAllAsync();
        Task<List<ResultTagDto>> GetAllTagsWithBlogCountAsync();
    }
}
