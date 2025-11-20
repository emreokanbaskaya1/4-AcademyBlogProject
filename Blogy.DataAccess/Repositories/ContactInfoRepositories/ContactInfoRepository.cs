using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.ContactInfoRepositories
{
    public class ContactInfoRepository : GenericRepository<ContactInfo>, IContactInfoRepository
    {
        public ContactInfoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
