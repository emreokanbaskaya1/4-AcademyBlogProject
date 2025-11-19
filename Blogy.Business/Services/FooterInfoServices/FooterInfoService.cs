using AutoMapper;
using Blogy.Business.DTOs.FooterInfoDtos;
using Blogy.DataAccess.Repositories.FooterInfoRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.FooterInfoServices
{
    public class FooterInfoService : IFooterInfoService
    {
        private readonly IFooterInfoRepository _footerInfoRepository;
        private readonly IMapper _mapper;

        public FooterInfoService(IFooterInfoRepository footerInfoRepository, IMapper mapper)
        {
            _footerInfoRepository = footerInfoRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultFooterInfoDto>> GetAllAsync()
        {
            var footerInfos = await _footerInfoRepository.GetAllAsync();
            return _mapper.Map<List<ResultFooterInfoDto>>(footerInfos);
        }

        public async Task<ResultFooterInfoDto> GetByIdAsync(int id)
        {
            var footerInfo = await _footerInfoRepository.GetByIdAsync(id);
            return _mapper.Map<ResultFooterInfoDto>(footerInfo);
        }

        public async Task<ResultFooterInfoDto> GetFirstAsync()
        {
            var footerInfos = await _footerInfoRepository.GetAllAsync();
            var firstFooterInfo = footerInfos.FirstOrDefault();
            return _mapper.Map<ResultFooterInfoDto>(firstFooterInfo);
        }

        public async Task CreateAsync(CreateFooterInfoDto dto)
        {
            var footerInfo = _mapper.Map<FooterInfo>(dto);
            await _footerInfoRepository.CreateAsync(footerInfo);
        }

        public async Task UpdateAsync(UpdateFooterInfoDto dto)
        {
            var footerInfo = _mapper.Map<FooterInfo>(dto);
            await _footerInfoRepository.UpdateAsync(footerInfo);
        }

        public async Task DeleteAsync(int id)
        {
            await _footerInfoRepository.DeleteAsync(id);
        }
    }
}
