using Blogy.Business.DTOs.DashboardDtos;

namespace Blogy.Business.Services.DashboardServices
{
    public interface IDashboardService
    {
        /// <summary>
        /// Dashboard için genel istatistikleri getirir
        /// </summary>
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        /// <summary>
        /// Kategorilere göre blog daðýlýmýný getirir (Pie Chart için)
        /// </summary>
        Task<List<CategoryDistributionDto>> GetCategoryDistributionAsync();

        /// <summary>
        /// Son N güne ait günlük aktivite verisini getirir (Line Chart için)
        /// </summary>
        /// <param name="days">Gün sayýsý (örn: 7, 30)</param>
        Task<List<DailyActivityDto>> GetDailyActivityAsync(int days = 7);
    }
}
