using AutoMapper;
using Blogy.Business.DTOs.AboutDtos;
using Blogy.DataAccess.Repositories.AboutRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.AboutServices
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IMapper _mapper;

        public AboutService(IAboutRepository aboutRepository, IMapper mapper)
        {
            _aboutRepository = aboutRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultAboutDto>> GetAllAsync()
        {
            var abouts = await _aboutRepository.GetAllAsync();
            return _mapper.Map<List<ResultAboutDto>>(abouts);
        }

        public async Task<ResultAboutDto> GetByIdAsync(int id)
        {
            var about = await _aboutRepository.GetByIdAsync(id);
            return _mapper.Map<ResultAboutDto>(about);
        }

        public async Task CreateAsync(CreateAboutDto dto)
        {
            var about = _mapper.Map<About>(dto);
            await _aboutRepository.CreateAsync(about);
        }

        public async Task UpdateAsync(UpdateAboutDto dto)
        {
            var about = _mapper.Map<About>(dto);
            await _aboutRepository.UpdateAsync(about);
        }

        public async Task DeleteAsync(int id)
        {
            await _aboutRepository.DeleteAsync(id);
        }
    }
}
