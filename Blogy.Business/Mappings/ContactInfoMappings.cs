using AutoMapper;
using Blogy.Business.DTOs.ContactInfoDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class ContactInfoMappings : Profile
    {
        public ContactInfoMappings()
        {
            CreateMap<ContactInfo, ResultContactInfoDto>().ReverseMap();
            CreateMap<ContactInfo, CreateContactInfoDto>().ReverseMap();
            CreateMap<ContactInfo, UpdateContactInfoDto>().ReverseMap();
        }
    }
}
