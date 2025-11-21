using Blogy.Business.DTOs.ContactMessageDtos;
using Blogy.Business.Services.ContactInfoServices;
using Blogy.Business.Services.ContactMessageServices;
using Blogy.Business.Services.OpenAIServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactInfoService _contactInfoService;
        private readonly IContactMessageService _contactMessageService;
        private readonly IOpenAIService _openAIService;

        public ContactController(
            IContactInfoService contactInfoService, 
            IContactMessageService contactMessageService,
            IOpenAIService openAIService)
        {
            _contactInfoService = contactInfoService;
            _contactMessageService = contactMessageService;
            _openAIService = openAIService;
        }

        public async Task<IActionResult> Index()
        {
            var contactInfo = await _contactInfoService.GetFirstAsync();
            ViewBag.ContactInfo = contactInfo;
            return View();
        }

        /// <summary>
        /// Ýletiþim formu mesajýný kaydeder ve AI ile çok dilli otomatik yanýt üretir
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateContactMessageDto model)
        {
            if (!ModelState.IsValid)
            {
                var contactInfo = await _contactInfoService.GetFirstAsync();
                ViewBag.ContactInfo = contactInfo;
                return View("Index", model);
            }

            try
            {
                // 1. Mesajý veritabanýna kaydet
                await _contactMessageService.CreateAsync(model);

                // 2. AI ile çok dilli otomatik yanýt üret
                // Kullanýcýnýn mesajýnýn dilini algýlar ve ayný dilde yanýt verir
                var autoReply = await _openAIService.GenerateMultilingualAutoReplyAsync(model.Message);

                // 3. Baþarý mesajýný kullanýcýya göster (AI'ýn ürettiði dilde)
                TempData["SuccessMessage"] = autoReply;
                
                return RedirectToAction("Index");
            }
            catch (Exception ex) when (ex.Message.Contains("rate limit") || ex.Message.Contains("temporarily unavailable"))
            {
                // Rate limit durumunda: Mesaj kaydedildi ama AI yanýtý yok
                Console.WriteLine($"[ContactController] ?? AI auto-reply bypassed due to rate limit");
                
                // Fallback: Ýngilizce varsayýlan mesaj
                TempData["SuccessMessage"] = "Your message has been sent successfully! We will get back to you soon.";
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Diðer hatalar
                Console.WriteLine($"[ContactController] ? Error: {ex.Message}");
                
                // Fallback: Ýngilizce varsayýlan mesaj
                TempData["SuccessMessage"] = "Your message has been sent successfully!";
                
                return RedirectToAction("Index");
            }
        }
    }
}
