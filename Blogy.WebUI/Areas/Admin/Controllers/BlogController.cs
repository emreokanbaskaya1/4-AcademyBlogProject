using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.OpenAIDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.OpenAIServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOpenAIService _openAIService;

        public BlogController(
            IBlogService blogService, 
            ICategoryService categoryService, 
            UserManager<AppUser> userManager,
            IOpenAIService openAIService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _userManager = userManager;
            _openAIService = openAIService;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllAsync(); //Lazy loading
            return View(blogs);
        }

        public async Task<IActionResult> CreateBlog()
        {
            await GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(CreateBlogDto blogDto)
        {
            if (!ModelState.IsValid) 
            {
                await GetCategoriesAsync();
                return View(blogDto);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            blogDto.WriterId = user.Id;


            await _blogService.CreateAsync(blogDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteBlog(int id)
        {
            await _blogService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateBlog(int id)
        {
            await GetCategoriesAsync();
            var blog = await _blogService.GetById(id);
            return View(blog);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlog(UpdateBlogDto updateBlogDto)
        {
            if (!ModelState.IsValid)
            {
                await GetCategoriesAsync();
                return View(updateBlogDto);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            updateBlogDto.WriterId = user.Id;

            await _blogService.UpdateAsync(updateBlogDto);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Generates blog article content using AI
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GenerateArticle([FromBody] GenerateArticleRequestDto request)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(request.Keywords) || string.IsNullOrWhiteSpace(request.ShortDescription))
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = "Keywords and short description fields are required." 
                    });
                }

                // Generate article with AI
                var generatedContent = await _openAIService.GenerateArticleAsync(
                    request.Keywords, 
                    request.ShortDescription
                );

                // Return success result
                return Json(new 
                { 
                    success = true, 
                    content = generatedContent 
                });
            }
            catch (Exception ex)
            {
                // Return error message to user
                return Json(new 
                { 
                    success = false, 
                    message = $"Error generating article: {ex.Message}" 
                });
            }
        }

        private async Task GetCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = (from category in categories
                                  select new SelectListItem
                                  {
                                      Text = category.CategoryName,
                                      Value = category.Id.ToString()
                                  }).ToList();
        }
    }
}
