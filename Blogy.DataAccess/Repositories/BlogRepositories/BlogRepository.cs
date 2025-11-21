using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogy.DataAccess.Repositories.BlogRepositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Blog>> GetBlogsWithCategoriesAsync()
        {
            return await _table.Include(x=>x.Category).ToListAsync();
        }

        public async Task<List<Blog>> GetLast3BlogsAsync()
        {
            return await _table.OrderByDescending(x=>x.Id).Take(3).ToListAsync();
        }

        public async Task<List<Blog>> GetLast5BlogsAsync()
        {
            return await _table
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToListAsync();
        }

        public async Task<Blog> GetBlogDetailsByIdAsync(int id)
        {
            return await _table
                .Include(x => x.Category)          // Category bilgilerini yükle
                .Include(x => x.Writer)            // Writer bilgilerini yükle
                .Include(x => x.Comments)          // Comment'leri yükle
                    .ThenInclude(c => c.User)      // Her comment'in User bilgisini yükle
                .Include(x => x.BlogTags)          // BlogTags'leri yükle
                    .ThenInclude(bt => bt.Tag)     // Her BlogTag'in Tag bilgisini yükle
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
