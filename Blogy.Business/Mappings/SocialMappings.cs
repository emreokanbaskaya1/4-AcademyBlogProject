using AutoMapper;
using Blogy.Business.DTOs.SocialDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class SocialMappings : Profile
    {
        public SocialMappings()
        {
            CreateMap<Social, ResultSocialDto>().ReverseMap();
            CreateMap<Social, CreateSocialDto>().ReverseMap();
            CreateMap<Social, UpdateSocialDto>().ReverseMap();
        }
    }
}
