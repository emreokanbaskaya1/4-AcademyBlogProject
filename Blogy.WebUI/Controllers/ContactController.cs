using Blogy.Business.DTOs.ContactMessageDtos;
using Blogy.Business.Services.ContactInfoServices;
using Blogy.Business.Services.ContactMessageServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactInfoService _contactInfoService;
        private readonly IContactMessageService _contactMessageService;

        public ContactController(IContactInfoService contactInfoService, IContactMessageService contactMessageService)
        {
            _contactInfoService = contactInfoService;
            _contactMessageService = contactMessageService;
        }

        public async Task<IActionResult> Index()
        {
            var contactInfo = await _contactInfoService.GetFirstAsync();
            ViewBag.ContactInfo = contactInfo;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateContactMessageDto model)
        {
            if (!ModelState.IsValid)
            {
                var contactInfo = await _contactInfoService.GetFirstAsync();
                ViewBag.ContactInfo = contactInfo;
                return View("Index", model);
            }

            await _contactMessageService.CreateAsync(model);
            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }
    }
}
