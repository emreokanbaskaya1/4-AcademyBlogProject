using Blogy.Business.Services.FooterInfoServices;
using Blogy.Business.Services.SocialServices;
using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISocialService _socialService;
        private readonly IFooterInfoService _footerInfoService;
        private readonly IBlogService _blogService;

        public FooterViewComponent(ISocialService socialService, IFooterInfoService footerInfoService, IBlogService blogService)
        {
            _socialService = socialService;
            _footerInfoService = footerInfoService;
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Socials = await _socialService.GetAllAsync();
            ViewBag.FooterInfo = await _footerInfoService.GetFirstAsync();
            ViewBag.LatestBlogs = await _blogService.GetLast3BlogsAsync();
            return View();
        }
    }
}
