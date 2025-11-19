using Blogy.Business.Services.AboutServices;
using Blogy.Business.Services.TeamMemberServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly ITeamMemberService _teamMemberService;

        public AboutController(IAboutService aboutService, ITeamMemberService teamMemberService)
        {
            _aboutService = aboutService;
            _teamMemberService = teamMemberService;
        }

        public async Task<IActionResult> Index()
        {
            // Hakkýmýzda bilgilerini al
            var abouts = await _aboutService.GetAllAsync();
            ViewBag.Abouts = abouts;

            // Ekip üyelerini al
            var teamMembers = await _teamMemberService.GetAllAsync();
            ViewBag.TeamMembers = teamMembers;

            return View();
        }
    }
}
