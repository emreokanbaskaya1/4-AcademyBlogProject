using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.ToxicityServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IBlogService _blogService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToxicityService _toxicityService;

        public CommentController(
            ICommentService commentService,
            IBlogService blogService,
            UserManager<AppUser> userManager,
            IToxicityService toxicityService)
        {
            _commentService = commentService;
            _blogService = blogService;
            _userManager = userManager;
            _toxicityService = toxicityService;
        }

        private async Task GetBlogs()
        {
            var blogs = await _blogService.GetAllAsync();
            ViewBag.blogs = (from blog in blogs
                             select new SelectListItem
                             {
                                 Text = blog.Title,
                                 Value = blog.Id.ToString()
                             }).ToList();
        }        
        
        public async Task<IActionResult> Index()
        {
            var comments = await _commentService.GetAllAsync();
            return View(comments);
        }

        public async Task<IActionResult> CreateComment()
        {
            await GetBlogs();
            return View();
        }

        /// <summary>
        /// Admin panelinden yorum ekleme (OpenAI Moderation ile toxicity kontrolü)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            try
            {
                // AI ile toxicity analizi
                var toxicityResult = await _toxicityService.AnalyzeCommentAsync(createCommentDto.Content);

                // Eğer yorum toksikse, hata göster
                if (toxicityResult.IsToxic)
                {
                    ModelState.AddModelError("Content", toxicityResult.Message);
                    await GetBlogs();
                    return View(createCommentDto);
                }

                // Yorum temizse kaydet
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                createCommentDto.UserId = user.Id;
                await _commentService.CreateAsync(createCommentDto);

                TempData["Success"] = "Comment created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex.Message.Contains("rate limit") || ex.Message.Contains("temporarily unavailable"))
            {
                // Rate limit: Yorumu yine de kaydet
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                createCommentDto.UserId = user.Id;
                await _commentService.CreateAsync(createCommentDto);

                TempData["Warning"] = "⚠️ Comment posted without AI moderation. Will be reviewed.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                await GetBlogs();
                return View(createCommentDto);
            }
        }

        /// <summary>
        /// Admin herhangi bir yorumu silebilir (kullanıcı kontrolü yok)
        /// </summary>
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteAsync(id);
            TempData["Success"] = "Comment deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
