using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
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
    public class BlogController(IBlogService _blogService, ICategoryService _categoryService, UserManager<AppUser> _userManager) : Controller
    {
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

        public async Task<IActionResult> Index()
        {
            // Writer sadece kendi bloglarını görsün
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allBlogs = await _blogService.GetAllAsync();
            var myBlogs = allBlogs.Where(x => x.WriterId == user.Id).ToList();
            return View(myBlogs);
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
            // Sadece kendi blogunu silebilsin
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var blog = await _blogService.GetSingleByIdAsync(id);
            
            if (blog.WriterId != user.Id)
            {
                return Forbid(); // 403 Forbidden
            }

            await _blogService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateBlog(int id)
        {
            // Sadece kendi blogunu güncelleyebilsin
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var blog = await _blogService.GetById(id);

            if (blog.WriterId != user.Id)
            {
                return Forbid();
            }

            await GetCategoriesAsync();
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
    }
}
