using Blogy.Business.DTOs.DashboardDtos;
using Blogy.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Blogy.Business.Services.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dashboard için genel istatistikleri getirir
        /// </summary>
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var stats = new DashboardStatsDto
            {
                // Toplam blog sayýsý
                TotalBlogs = await _context.Blogs.CountAsync(),

                // Toplam kullanýcý sayýsý
                TotalUsers = await _context.Users.CountAsync(),

                // Toplam yorum sayýsý
                TotalComments = await _context.Comments.CountAsync(),

                // Okunmamýþ mesaj sayýsý (tüm mesajlar)
                TotalMessages = await _context.ContactMessages.CountAsync(),

                // Toplam kategori sayýsý
                TotalCategories = await _context.Categories.CountAsync(),

                // Son 30 günde blog yazan aktif yazar sayýsý
                TotalActiveWriters = await _context.Blogs
                    .Where(b => b.CreatedDate >= DateTime.Now.AddDays(-30))
                    .Select(b => b.WriterId)
                    .Distinct()
                    .CountAsync()
            };

            return stats;
        }

        /// <summary>
        /// Kategorilere göre blog daðýlýmýný getirir (Pie Chart için)
        /// </summary>
        public async Task<List<CategoryDistributionDto>> GetCategoryDistributionAsync()
        {
            var totalBlogs = await _context.Blogs.CountAsync();

            if (totalBlogs == 0)
            {
                return new List<CategoryDistributionDto>();
            }

            var distribution = await _context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BlogCount = _context.Blogs.Count(b => b.CategoryId == c.Id)
                })
                .Where(x => x.BlogCount > 0) // Sadece blog içeren kategoriler
                .OrderByDescending(x => x.BlogCount)
                .ToListAsync();

            var result = distribution.Select(d => new CategoryDistributionDto
            {
                CategoryName = d.CategoryName,
                BlogCount = d.BlogCount,
                Percentage = Math.Round((double)d.BlogCount / totalBlogs * 100, 1)
            }).ToList();

            return result;
        }

        /// <summary>
        /// Son N güne ait günlük aktivite verisini getirir (Line Chart için)
        /// </summary>
        public async Task<List<DailyActivityDto>> GetDailyActivityAsync(int days = 7)
        {
            var startDate = DateTime.Now.AddDays(-days).Date;
            var endDate = DateTime.Now.Date;

            var dailyActivities = new List<DailyActivityDto>();

            // Her gün için veri oluþtur
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var nextDay = date.AddDays(1);

                var blogCount = await _context.Blogs
                    .Where(b => b.CreatedDate >= date && b.CreatedDate < nextDay)
                    .CountAsync();

                var commentCount = await _context.Comments
                    .Where(c => c.CreatedDate >= date && c.CreatedDate < nextDay)
                    .CountAsync();

                dailyActivities.Add(new DailyActivityDto
                {
                    Date = date,
                    BlogCount = blogCount,
                    CommentCount = commentCount
                });
            }

            return dailyActivities;
        }
    }
}
