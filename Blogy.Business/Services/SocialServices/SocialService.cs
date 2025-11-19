using AutoMapper;
using Blogy.Business.DTOs.SocialDtos;
using Blogy.DataAccess.Repositories.SocialRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.SocialServices
{
    public class SocialService : ISocialService
    {
        private readonly ISocialRepository _socialRepository;
        private readonly IMapper _mapper;

        public SocialService(ISocialRepository socialRepository, IMapper mapper)
        {
            _socialRepository = socialRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultSocialDto>> GetAllAsync()
        {
            var socials = await _socialRepository.GetAllAsync();
            return _mapper.Map<List<ResultSocialDto>>(socials);
        }

        public async Task<ResultSocialDto> GetByIdAsync(int id)
        {
            var social = await _socialRepository.GetByIdAsync(id);
            return _mapper.Map<ResultSocialDto>(social);
        }

        public async Task CreateAsync(CreateSocialDto dto)
        {
            var social = _mapper.Map<Social>(dto);
            await _socialRepository.CreateAsync(social);
        }

        public async Task UpdateAsync(UpdateSocialDto dto)
        {
            var social = _mapper.Map<Social>(dto);
            await _socialRepository.UpdateAsync(social);
        }

        public async Task DeleteAsync(int id)
        {
            await _socialRepository.DeleteAsync(id);
        }
    }
}
