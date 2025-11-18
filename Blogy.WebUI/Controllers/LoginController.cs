using Blogy.Business.DTOs.UserDtos;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class LoginController(SignInManager<AppUser> _signInManager, UserManager<AppUser> _userManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username or password is not correct");
                return View(model);
            }

            // Kullanıcıyı bul ve rollerini al
            var user = await _userManager.FindByNameAsync(model.Username);
            var roles = await _userManager.GetRolesAsync(user);

            // İlk role göre yönlendirme yap
            if (roles.Contains(Roles.Admin))
            {
                return RedirectToAction("Index", "Blog");
            }
            else if (roles.Contains(Roles.Writer))
            {
                return RedirectToAction("Index", "Blog");
            }
            else if (roles.Contains(Roles.User))
            {
                return RedirectToAction("Index", "Blog");
            }

            // Rolü yoksa ana sayfaya yönlendir
            return RedirectToAction("Index", "Default");
        }
    }
}
