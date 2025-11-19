using Blogy.Business.DTOs.FooterInfoDtos;
using Blogy.Business.Services.FooterInfoServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class FooterInfoController(IFooterInfoService _footerInfoService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var footerInfos = await _footerInfoService.GetAllAsync();
            return View(footerInfos);
        }

        public IActionResult CreateFooterInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFooterInfo(CreateFooterInfoDto footerInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(footerInfoDto);
            }

            await _footerInfoService.CreateAsync(footerInfoDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateFooterInfo(int id)
        {
            var footerInfo = await _footerInfoService.GetByIdAsync(id);
            var updateDto = new UpdateFooterInfoDto
            {
                Id = footerInfo.Id,
                AboutText = footerInfo.AboutText,
                CopyrightText = footerInfo.CopyrightText
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFooterInfo(UpdateFooterInfoDto updateFooterInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateFooterInfoDto);
            }

            await _footerInfoService.UpdateAsync(updateFooterInfoDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFooterInfo(int id)
        {
            await _footerInfoService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
