using Blogy.Business.DTOs.FooterInfoDtos;
using Blogy.Business.Services.FooterInfoServices;
using Blogy.Business.Services.OpenAIServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class FooterInfoController : Controller
    {
        private readonly IFooterInfoService _footerInfoService;
        private readonly IOpenAIService _openAIService;

        public FooterInfoController(IFooterInfoService footerInfoService, IOpenAIService openAIService)
        {
            _footerInfoService = footerInfoService;
            _openAIService = openAIService;
        }

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

        /// <summary>
        /// AI ile About Text üretir (AJAX ile çaðrýlýr)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GenerateAboutText([FromBody] GenerateAboutTextRequest request)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(request.CompanyName) || string.IsNullOrWhiteSpace(request.Keywords))
                {
                    return Json(new { success = false, message = "Company name and keywords are required!" });
                }

                // AI ile about text üret
                var generatedText = await _openAIService.GenerateAboutTextAsync(request.CompanyName, request.Keywords);

                return Json(new { success = true, aboutText = generatedText });
            }
            catch (Exception ex) when (ex.Message.Contains("rate limit") || ex.Message.Contains("temporarily unavailable"))
            {
                return Json(new { success = false, message = "?? AI service is temporarily unavailable. Please try again later." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }

    // Request model for AI generation
    public class GenerateAboutTextRequest
    {
        public string CompanyName { get; set; }
        public string Keywords { get; set; }
    }
}
