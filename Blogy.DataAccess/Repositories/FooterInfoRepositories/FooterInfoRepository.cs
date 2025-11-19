using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.FooterInfoRepositories
{
    public class FooterInfoRepository : GenericRepository<FooterInfo>, IFooterInfoRepository
    {
        public FooterInfoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
