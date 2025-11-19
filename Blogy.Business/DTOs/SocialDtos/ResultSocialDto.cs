using Blogy.Business.DTOs.Common;

namespace Blogy.Business.DTOs.SocialDtos
{
    public class ResultSocialDto : BaseDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}
