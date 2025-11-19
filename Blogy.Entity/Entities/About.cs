using Blogy.Entity.Entities.Common;

namespace Blogy.Entity.Entities
{
    public class About : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
