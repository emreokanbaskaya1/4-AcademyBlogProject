using Blogy.Business.DTOs.Common;

namespace Blogy.Business.DTOs.ContactInfoDtos
{
    public class ResultContactInfoDto : BaseDto
    {
        public string Location { get; set; }
        public string OpenHours { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
