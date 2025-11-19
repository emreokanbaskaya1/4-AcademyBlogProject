using AutoMapper;
using Blogy.Business.DTOs.TeamMemberDtos;
using Blogy.DataAccess.Repositories.TeamMemberRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.TeamMemberServices
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IMapper _mapper;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository, IMapper mapper)
        {
            _teamMemberRepository = teamMemberRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultTeamMemberDto>> GetAllAsync()
        {
            var teamMembers = await _teamMemberRepository.GetAllAsync();
            return _mapper.Map<List<ResultTeamMemberDto>>(teamMembers);
        }

        public async Task<ResultTeamMemberDto> GetByIdAsync(int id)
        {
            var teamMember = await _teamMemberRepository.GetByIdAsync(id);
            return _mapper.Map<ResultTeamMemberDto>(teamMember);
        }

        public async Task CreateAsync(CreateTeamMemberDto dto)
        {
            var teamMember = _mapper.Map<TeamMember>(dto);
            await _teamMemberRepository.CreateAsync(teamMember);
        }

        public async Task UpdateAsync(UpdateTeamMemberDto dto)
        {
            var teamMember = _mapper.Map<TeamMember>(dto);
            await _teamMemberRepository.UpdateAsync(teamMember);
        }

        public async Task DeleteAsync(int id)
        {
            await _teamMemberRepository.DeleteAsync(id);
        }
    }
}
