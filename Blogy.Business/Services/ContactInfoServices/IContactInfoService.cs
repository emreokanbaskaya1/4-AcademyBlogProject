using Blogy.Business.DTOs.ContactInfoDtos;

namespace Blogy.Business.Services.ContactInfoServices
{
    public interface IContactInfoService
    {
        Task<List<ResultContactInfoDto>> GetAllAsync();
        Task<ResultContactInfoDto> GetByIdAsync(int id);
        Task<ResultContactInfoDto> GetFirstAsync();
        Task CreateAsync(CreateContactInfoDto dto);
        Task UpdateAsync(UpdateContactInfoDto dto);
        Task DeleteAsync(int id);
    }
}
