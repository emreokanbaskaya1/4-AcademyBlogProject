using AutoMapper;
using Blogy.Business.DTOs.TeamMemberDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings
{
    public class TeamMemberMappings : Profile
    {
        public TeamMemberMappings()
        {
            CreateMap<TeamMember, ResultTeamMemberDto>().ReverseMap();
            CreateMap<TeamMember, CreateTeamMemberDto>().ReverseMap();
            CreateMap<TeamMember, UpdateTeamMemberDto>().ReverseMap();
        }
    }
}
