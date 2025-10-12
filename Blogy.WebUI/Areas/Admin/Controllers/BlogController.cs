using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    public class BlogController(IBlogService _blogService) : Controller
    {
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetBlogWithCategoriesAsync();
            return View(blogs);
        }
    }
}
