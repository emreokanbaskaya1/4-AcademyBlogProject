using Blogy.Business.DTOs.TeamMemberDtos;

namespace Blogy.Business.Services.TeamMemberServices
{
    public interface ITeamMemberService
    {
        Task<List<ResultTeamMemberDto>> GetAllAsync();
        Task<ResultTeamMemberDto> GetByIdAsync(int id);
        Task CreateAsync(CreateTeamMemberDto dto);
        Task UpdateAsync(UpdateTeamMemberDto dto);
        Task DeleteAsync(int id);
    }
}
