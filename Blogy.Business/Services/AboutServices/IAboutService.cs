using Blogy.Business.DTOs.AboutDtos;

namespace Blogy.Business.Services.AboutServices
{
    public interface IAboutService
    {
        Task<List<ResultAboutDto>> GetAllAsync();
        Task<ResultAboutDto> GetByIdAsync(int id);
        Task CreateAsync(CreateAboutDto dto);
        Task UpdateAsync(UpdateAboutDto dto);
        Task DeleteAsync(int id);
    }
}
