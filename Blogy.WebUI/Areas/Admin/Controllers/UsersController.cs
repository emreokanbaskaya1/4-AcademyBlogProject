using AutoMapper;
using Blogy.Business.DTOs.UserDtos;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class UsersController(UserManager<AppUser> _userManager, IMapper _mapper, RoleManager<AppRole> _roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            
            var mappedUsers = _mapper.Map<List<ResultUserDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                var userRoles = await _userManager.GetRolesAsync(users[i]);
                mappedUsers[i].Roles = userRoles.ToList();
            }

            return View(mappedUsers);
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()); //Id ye göre kullanıcıyı bulduk
            ViewBag.fullName = user.FirstName + " " + user.LastName;
            var roles = await _roleManager.Roles.ToListAsync();  // Bütün roller listeledik
            var userRoles = await _userManager.GetRolesAsync(user); //O kullanıcıya ait roller
            var assignRoleList = new List<AssignRoleDto>(); //Dto yapısı


            foreach(var role in roles)
            {
                assignRoleList.Add(new AssignRoleDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    UserId = user.Id,
                    RoleExist = userRoles.Contains(role.Name)
                });
            }

            return View(assignRoleList);
        }

        [HttpPost] 
        public async Task<IActionResult> AssignRole(List<AssignRoleDto> model)
        {
            var userId = model.Select(x => x.UserId).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            foreach(var role in model)
            {
                if(role.RoleExist == true)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }

            return RedirectToAction("Index");
        }
    }

}
