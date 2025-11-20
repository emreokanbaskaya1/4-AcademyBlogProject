using Blogy.Business.DTOs.ContactMessageDtos;

namespace Blogy.Business.Services.ContactMessageServices
{
    public interface IContactMessageService
    {
        Task<List<ResultContactMessageDto>> GetAllAsync();
        Task<ResultContactMessageDto> GetByIdAsync(int id);
        Task<List<ResultContactMessageDto>> GetUnreadMessagesAsync();
        Task CreateAsync(CreateContactMessageDto dto);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
