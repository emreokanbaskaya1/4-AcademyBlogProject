using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.BlogRepositories
{
    public interface IBlogRepository: IGenericRepository<Blog>
    {
        Task<List<Blog>> GetBlogsWithCategoriesAsync();

        Task<List<Blog>> GetLast3BlogsAsync();

        Task<List<Blog>> GetLast5BlogsAsync();

        Task<Blog> GetBlogDetailsByIdAsync(int id);
    }
}
