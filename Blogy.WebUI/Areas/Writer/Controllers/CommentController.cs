using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CommentServices;
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
    public class CommentController(ICommentService _commentService, 
                                   IBlogService _blogService, 
                                   UserManager<AppUser> _userManager) : Controller
    {
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

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                await GetBlogs();
                return View(createCommentDto);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            createCommentDto.UserId = user.Id;
            await _commentService.CreateAsync(createCommentDto);
            return RedirectToAction(nameof(MyComments));
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            // Sadece kendi yorumunu silebilsin
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
