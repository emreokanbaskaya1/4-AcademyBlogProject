using Blogy.Business.DTOs.Common;

namespace Blogy.Business.DTOs.ContactMessageDtos
{
    public class ResultContactMessageDto : BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
