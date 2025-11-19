using Blogy.Business.DTOs.Common;

namespace Blogy.Business.DTOs.FooterInfoDtos
{
    public class UpdateFooterInfoDto : BaseDto
    {
        public string AboutText { get; set; }
        public string CopyrightText { get; set; }
    }
}
