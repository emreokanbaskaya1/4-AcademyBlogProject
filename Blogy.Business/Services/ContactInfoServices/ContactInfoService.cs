using AutoMapper;
using Blogy.Business.DTOs.ContactInfoDtos;
using Blogy.DataAccess.Repositories.ContactInfoRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.ContactInfoServices
{
    public class ContactInfoService : IContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;

        public ContactInfoService(IContactInfoRepository contactInfoRepository, IMapper mapper)
        {
            _contactInfoRepository = contactInfoRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultContactInfoDto>> GetAllAsync()
        {
            var contactInfos = await _contactInfoRepository.GetAllAsync();
            return _mapper.Map<List<ResultContactInfoDto>>(contactInfos);
        }

        public async Task<ResultContactInfoDto> GetByIdAsync(int id)
        {
            var contactInfo = await _contactInfoRepository.GetByIdAsync(id);
            return _mapper.Map<ResultContactInfoDto>(contactInfo);
        }

        public async Task<ResultContactInfoDto> GetFirstAsync()
        {
            var contactInfos = await _contactInfoRepository.GetAllAsync();
            var firstContactInfo = contactInfos.FirstOrDefault();
            return _mapper.Map<ResultContactInfoDto>(firstContactInfo);
        }

        public async Task CreateAsync(CreateContactInfoDto dto)
        {
            var contactInfo = _mapper.Map<ContactInfo>(dto);
            await _contactInfoRepository.CreateAsync(contactInfo);
        }

        public async Task UpdateAsync(UpdateContactInfoDto dto)
        {
            var contactInfo = _mapper.Map<ContactInfo>(dto);
            await _contactInfoRepository.UpdateAsync(contactInfo);
        }

        public async Task DeleteAsync(int id)
        {
            await _contactInfoRepository.DeleteAsync(id);
        }
    }
}
