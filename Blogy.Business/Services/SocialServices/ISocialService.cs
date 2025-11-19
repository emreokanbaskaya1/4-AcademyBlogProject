using Blogy.Business.DTOs.SocialDtos;

namespace Blogy.Business.Services.SocialServices
{
    public interface ISocialService
    {
        Task<List<ResultSocialDto>> GetAllAsync();
        Task<ResultSocialDto> GetByIdAsync(int id);
        Task CreateAsync(CreateSocialDto dto);
        Task UpdateAsync(UpdateSocialDto dto);
        Task DeleteAsync(int id);
    }
}
