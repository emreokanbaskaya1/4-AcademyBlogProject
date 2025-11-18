using AutoMapper;
using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.UserDtos;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blogy.Business.Mappings
{
    
    public class UserMappings: Profile
    {
        public UserMappings() {
            // Use string concatenation instead of string.Join to avoid params/ReadOnlySpan issues in expression trees
            CreateMap<AppUser, ResultUserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<AppUser, EditProfileDto>().ReverseMap();
        }
    }
}
