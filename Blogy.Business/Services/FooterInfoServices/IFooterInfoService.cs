using Blogy.Business.DTOs.FooterInfoDtos;

namespace Blogy.Business.Services.FooterInfoServices
{
    public interface IFooterInfoService
    {
        Task<List<ResultFooterInfoDto>> GetAllAsync();
        Task<ResultFooterInfoDto> GetByIdAsync(int id);
        Task<ResultFooterInfoDto> GetFirstAsync();
        Task CreateAsync(CreateFooterInfoDto dto);
        Task UpdateAsync(UpdateFooterInfoDto dto);
        Task DeleteAsync(int id);
    }
}
