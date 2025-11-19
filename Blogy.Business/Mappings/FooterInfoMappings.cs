using AutoMapper;
using Blogy.Business.DTOs.FooterInfoDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class FooterInfoMappings : Profile
    {
        public FooterInfoMappings()
        {
            CreateMap<FooterInfo, ResultFooterInfoDto>().ReverseMap();
            CreateMap<FooterInfo, CreateFooterInfoDto>().ReverseMap();
            CreateMap<FooterInfo, UpdateFooterInfoDto>().ReverseMap();
        }
    }
}
