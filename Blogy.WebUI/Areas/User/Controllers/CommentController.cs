using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.CommentServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = Roles.User)]
    public class CommentController(ICommentService _commentService, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> MyComments()
        {
            // Giriş yapan kullanıcının yorumlarını getir
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allComments = await _commentService.GetAllAsync();
            var myComments = allComments.Where(x => x.UserId == user.Id).ToList();

            return View(myComments);
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            // Sadece kendi yorumunu silebilsin
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var comment = await _commentService.GetSingleByIdAsync(id);

            if (comment.UserId != user.Id)
            {
                return Forbid(); // 403 Forbidden
            }

            await _commentService.DeleteAsync(id);
            return RedirectToAction(nameof(MyComments));
        }
    }
}