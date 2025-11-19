using Blogy.Business.DTOs.SocialDtos;
using Blogy.Business.Services.SocialServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class SocialController(ISocialService _socialService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var socials = await _socialService.GetAllAsync();
            return View(socials);
        }

        public IActionResult CreateSocial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSocial(CreateSocialDto socialDto)
        {
            if (!ModelState.IsValid)
            {
                return View(socialDto);
            }

            await _socialService.CreateAsync(socialDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateSocial(int id)
        {
            var social = await _socialService.GetByIdAsync(id);
            var updateDto = new UpdateSocialDto
            {
                Id = social.Id,
                Name = social.Name,
                Url = social.Url,
                Icon = social.Icon
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSocial(UpdateSocialDto updateSocialDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateSocialDto);
            }

            await _socialService.UpdateAsync(updateSocialDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSocial(int id)
        {
            await _socialService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
