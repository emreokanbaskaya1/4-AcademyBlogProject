using AutoMapper;
using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.TagServices;
using Blogy.Business.Services.ToxicityServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Threading.Tasks;

namespace Blogy.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly ITagService _tagService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IToxicityService _toxicityService;

        public BlogController(
            IBlogService blogService,
            ICategoryService categoryService,
            ICommentService commentService,
            ITagService tagService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IToxicityService toxicityService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _commentService = commentService;
            _tagService = tagService;
            _userManager = userManager;
            _mapper = mapper;
            _toxicityService = toxicityService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 2)
        {
            var blogs = await _blogService.GetAllAsync();
            var values = new PagedList<ResultBlogDto>(blogs.AsQueryable(), page, pageSize);

            // Sidebar için veri hazırlama
            // 1. En son eklenen 3 blog
            var latestBlogs = await _blogService.GetLast3BlogsAsync();
            ViewBag.LatestBlogs = latestBlogs;

            // 2. Kategoriler ve blog sayıları
            var categories = await _categoryService.GetCategoriesWithBlogs();
            ViewBag.Categories = categories;

            // 3. Tüm tag'ler ve blog sayıları
            var tags = await _tagService.GetAllTagsWithBlogCountAsync();
            ViewBag.Tags = tags;

            return View(values);
        }

        public async Task<IActionResult> GetBlogsByCategory(int id) 
        { 
            var category = await _categoryService.GetByIdAsync(id);
            ViewBag.categoryName = category.Name;
            var blogs = await _blogService.GetBlogsByCategoryAsync(id);

            // Sidebar için veri hazırlama (Index ile aynı)
            var latestBlogs = await _blogService.GetLast3BlogsAsync();
            ViewBag.LatestBlogs = latestBlogs;

            var categories = await _categoryService.GetCategoriesWithBlogs();
            ViewBag.Categories = categories;

            var tags = await _tagService.GetAllTagsWithBlogCountAsync();
            ViewBag.Tags = tags;

            return View(blogs);
        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            var blog = await _blogService.GetSingleByIdAsync(id);

            // Sidebar için veri hazırlama
            var latestBlogs = await _blogService.GetLast3BlogsAsync();
            ViewBag.LatestBlogs = latestBlogs;

            var categories = await _categoryService.GetCategoriesWithBlogs();
            ViewBag.Categories = categories;

            var tags = await _tagService.GetAllTagsWithBlogCountAsync();
            ViewBag.Tags = tags;

            return View(blog);
        }

        /// <summary>
        /// Blog detay sayfasından yorum ekleme
        /// OpenAI Moderation API ile toxicity kontrolü
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto commentDto)
        {
            // 1. Login kontrolü
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                // 2. AI ile toxicity analizi yap
                var toxicityResult = await _toxicityService.AnalyzeCommentAsync(commentDto.Content);

                // 3. Eğer yorum toksikse, kullanıcıyı uyar
                if (toxicityResult.IsToxic)
                {
                    TempData["ToxicityError"] = toxicityResult.Message;
                    
                    // DÜZELTME: Double'ı string'e çevir
                    TempData["ToxicityScore"] = toxicityResult.ToxicityScore.ToString("F2");
                    
                    return RedirectToAction("BlogDetails", new { id = commentDto.BlogId });
                }

                // 4. Yorum temizse, kaydet
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                commentDto.UserId = user.Id;

                await _commentService.CreateAsync(commentDto);

                TempData["CommentSuccess"] = "Your comment has been posted successfully!";

                return RedirectToAction("BlogDetails", new { id = commentDto.BlogId });
            }
            catch (Exception ex) when (ex.Message.Contains("rate limit") || ex.Message.Contains("temporarily unavailable"))
            {
                // Rate limit durumunda: Yorumu toxicity kontrolü OLMADAN kaydet
                Console.WriteLine($"[BlogController] Toxicity check bypassed due to rate limit. Posting comment anyway.");
                
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                commentDto.UserId = user.Id;

                await _commentService.CreateAsync(commentDto);

                TempData["CommentWarning"] = "⚠️ Your comment was posted without AI moderation due to temporary service limitations. It will be reviewed by moderators.";
                
                return RedirectToAction("BlogDetails", new { id = commentDto.BlogId });
            }
            catch (Exception ex)
            {
                // Diğer hatalar
                Console.WriteLine($"[BlogController] Error: {ex.Message}");
                TempData["Error"] = "An error occurred while posting your comment. Please try again.";
                
                return RedirectToAction("BlogDetails", new { id = commentDto.BlogId });
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
