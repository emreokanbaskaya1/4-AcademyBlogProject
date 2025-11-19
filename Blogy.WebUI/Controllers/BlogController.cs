using AutoMapper;
using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.TagServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Threading.Tasks;

namespace Blogy.WebUI.Controllers
{
    public class BlogController(IBlogService _blogService, 
                                ICategoryService _categoryService, 
                                ICommentService _commentService,
                                ITagService _tagService,
                                UserManager<AppUser> _userManager,
                                IMapper _mapper) : Controller
    {
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

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto commentDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            commentDto.UserId = user.Id;

            await _commentService.CreateAsync(commentDto);

            return RedirectToAction("BlogDetails", new { id = commentDto.BlogId });
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
