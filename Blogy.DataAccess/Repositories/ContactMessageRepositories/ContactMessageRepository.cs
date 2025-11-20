using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.ContactMessageRepositories
{
    public class ContactMessageRepository : GenericRepository<ContactMessage>, IContactMessageRepository
    {
        public ContactMessageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
