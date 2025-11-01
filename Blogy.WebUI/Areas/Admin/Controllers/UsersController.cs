using AutoMapper;
using Blogy.Business.DTOs.UserDtos;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    public class UsersController(UserManager<AppUser> _userManager, IMapper _mapper) : Controller
    {
        [Area(Roles.Admin)]
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
    }
}
