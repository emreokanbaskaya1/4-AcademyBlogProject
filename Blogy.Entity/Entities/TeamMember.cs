using Blogy.Entity.Entities.Common;

namespace Blogy.Entity.Entities
{
    public class TeamMember : BaseEntity
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
