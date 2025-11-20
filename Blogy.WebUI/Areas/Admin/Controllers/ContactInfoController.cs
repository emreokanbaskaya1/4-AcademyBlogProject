using Blogy.Business.DTOs.ContactInfoDtos;
using Blogy.Business.Services.ContactInfoServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class ContactInfoController(IContactInfoService _contactInfoService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var contactInfos = await _contactInfoService.GetAllAsync();
            return View(contactInfos);
        }

        public IActionResult CreateContactInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactInfo(CreateContactInfoDto contactInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactInfoDto);
            }

            await _contactInfoService.CreateAsync(contactInfoDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateContactInfo(int id)
        {
            var contactInfo = await _contactInfoService.GetByIdAsync(id);
            var updateDto = new UpdateContactInfoDto
            {
                Id = contactInfo.Id,
                Location = contactInfo.Location,
                OpenHours = contactInfo.OpenHours,
                Email = contactInfo.Email,
                Phone = contactInfo.Phone
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContactInfo(UpdateContactInfoDto updateContactInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateContactInfoDto);
            }

            await _contactInfoService.UpdateAsync(updateContactInfoDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContactInfo(int id)
        {
            await _contactInfoService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
