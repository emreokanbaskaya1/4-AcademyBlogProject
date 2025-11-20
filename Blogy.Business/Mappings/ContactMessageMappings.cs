using AutoMapper;
using Blogy.Business.DTOs.ContactMessageDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class ContactMessageMappings : Profile
    {
        public ContactMessageMappings()
        {
            CreateMap<ContactMessage, ResultContactMessageDto>().ReverseMap();
            CreateMap<ContactMessage, CreateContactMessageDto>().ReverseMap();
        }
    }
}
