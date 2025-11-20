using Blogy.Business.Services.ContactMessageServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class ContactMessageController(IContactMessageService _contactMessageService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var messages = await _contactMessageService.GetAllAsync();
            return View(messages);
        }

        public async Task<IActionResult> Details(int id)
        {
            var message = await _contactMessageService.GetByIdAsync(id);
            await _contactMessageService.MarkAsReadAsync(id);
            return View(message);
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _contactMessageService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
