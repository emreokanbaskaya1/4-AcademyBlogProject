using Blogy.Business.Services.DashboardServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Admin Dashboard ana sayfasý
        /// Ýstatistik kartlarý + 2 dinamik chart içerir
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // 1. Ýstatistik kartlarý için veri
            var stats = await _dashboardService.GetDashboardStatsAsync();
            ViewBag.Stats = stats;

            // 2. Pie Chart için kategori daðýlýmý
            var categoryDistribution = await _dashboardService.GetCategoryDistributionAsync();
            ViewBag.CategoryDistribution = JsonSerializer.Serialize(categoryDistribution);

            // 3. Line Chart için son 7 günlük aktivite
            var dailyActivity = await _dashboardService.GetDailyActivityAsync(7);
            ViewBag.DailyActivity = JsonSerializer.Serialize(dailyActivity);

            return View();
        }
    }
}
