using AutoMapper;
using Blogy.Business.DTOs.UserDtos;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Writer.Controllers
{
    [Area("Writer")]
    [Authorize(Roles = $"{Roles.Writer}")]
    public class ProfileController(UserManager<AppUser> _userManager, IMapper _mapper, IWebHostEnvironment _webHostEnvironment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var editUser = _mapper.Map<EditProfileDto>(user);

            return View(editUser);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditProfileDto model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

            if (!passwordCorrect)
            {
                ModelState.AddModelError("", "The current password that was entered is not correct");
                return View(model);
            }

            if (model.ImageFile is not null)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName);
                var imageName = Guid.NewGuid() + extension;
                var saveLocation = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);

                var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                using var stream = new FileStream(saveLocation, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                user.ImageUrl = "/images/" + imageName;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Title = model.Title;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Update error");
                return View(model);

            }

            return RedirectToAction("Index", "Default", new { area = "" });



        }
    }
}
