using Blogy.Business.DTOs.Common;

namespace Blogy.Business.DTOs.AboutDtos
{
    public class ResultAboutDto : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
