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

namespace Blogy.WebUI.Areas.Writer.Controllers
{
    [Area("Writer")]
    [Authorize(Roles = Roles.Writer)]
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

        public async Task<IActionResult> MyComments()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allComments = await _commentService.GetAllAsync();
            var myComments = allComments.Where(x => x.UserId == user.Id).ToList();
            return View(myComments);
        }

        public async Task<IActionResult> CreateComment()
        {
            await GetBlogs();
            return View();
        }

        /// <summary>
        /// Writer panelinden yorum ekleme (OpenAI Moderation ile toxicity kontrolü)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                await GetBlogs();
                return View(createCommentDto);
            }

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

                TempData["Success"] = "Your comment has been posted successfully!";
                return RedirectToAction(nameof(MyComments));
            }
            catch (Exception ex) when (ex.Message.Contains("rate limit") || ex.Message.Contains("temporarily unavailable"))
            {
                // Rate limit: Yorumu yine de kaydet
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                createCommentDto.UserId = user.Id;
                await _commentService.CreateAsync(createCommentDto);

                TempData["Warning"] = "⚠️ Your comment was posted without AI moderation. It will be reviewed by moderators.";
                return RedirectToAction(nameof(MyComments));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                await GetBlogs();
                return View(createCommentDto);
            }
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var comment = await _commentService.GetSingleByIdAsync(id);

            if (comment.UserId != user.Id)
            {
                return Forbid();
            }

            await _commentService.DeleteAsync(id);
            return RedirectToAction(nameof(MyComments));
        }
    }
}
