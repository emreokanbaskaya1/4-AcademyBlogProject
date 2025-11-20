using AutoMapper;
using Blogy.Business.DTOs.ContactMessageDtos;
using Blogy.DataAccess.Repositories.ContactMessageRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.ContactMessageServices
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IMapper _mapper;

        public ContactMessageService(IContactMessageRepository contactMessageRepository, IMapper mapper)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultContactMessageDto>> GetAllAsync()
        {
            var messages = await _contactMessageRepository.GetAllAsync();
            return _mapper.Map<List<ResultContactMessageDto>>(messages.OrderByDescending(x => x.CreatedDate).ToList());
        }

        public async Task<ResultContactMessageDto> GetByIdAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            return _mapper.Map<ResultContactMessageDto>(message);
        }

        public async Task<List<ResultContactMessageDto>> GetUnreadMessagesAsync()
        {
            var messages = await _contactMessageRepository.GetAllAsync();
            var unreadMessages = messages.Where(x => !x.IsRead).OrderByDescending(x => x.CreatedDate).ToList();
            return _mapper.Map<List<ResultContactMessageDto>>(unreadMessages);
        }

        public async Task CreateAsync(CreateContactMessageDto dto)
        {
            var message = _mapper.Map<ContactMessage>(dto);
            message.IsRead = false;
            await _contactMessageRepository.CreateAsync(message);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _contactMessageRepository.UpdateAsync(message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _contactMessageRepository.DeleteAsync(id);
        }
    }
}
