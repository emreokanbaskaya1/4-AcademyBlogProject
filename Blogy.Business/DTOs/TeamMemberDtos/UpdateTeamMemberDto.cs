using Blogy.Business.DTOs.Common;

namespace Blogy.Business.DTOs.TeamMemberDtos
{
    public class UpdateTeamMemberDto : BaseDto
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
