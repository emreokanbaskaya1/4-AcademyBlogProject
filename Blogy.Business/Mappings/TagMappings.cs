using AutoMapper;
using Blogy.Business.DTOs.TagDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class TagMappings : Profile
    {
        public TagMappings()
        {
            CreateMap<Tag, ResultTagDto>()
                .ForMember(dest => dest.BlogCount, opt => opt.MapFrom(src => src.BlogTags.Count));
        }
    }
}
