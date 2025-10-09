using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.BlogRepositories
{
    public interface IBlogRepository: IGenericRepository<Blog>
    {
    }
}
