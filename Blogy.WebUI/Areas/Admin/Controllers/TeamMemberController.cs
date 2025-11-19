using Blogy.Business.DTOs.TeamMemberDtos;
using Blogy.Business.Services.TeamMemberServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class TeamMemberController(ITeamMemberService _teamMemberService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var teamMembers = await _teamMemberService.GetAllAsync();
            return View(teamMembers);
        }

        public IActionResult CreateTeamMember()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamMember(CreateTeamMemberDto teamMemberDto)
        {
            if (!ModelState.IsValid)
            {
                return View(teamMemberDto);
            }

            await _teamMemberService.CreateAsync(teamMemberDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateTeamMember(int id)
        {
            var teamMember = await _teamMemberService.GetByIdAsync(id);
            var updateDto = new UpdateTeamMemberDto
            {
                Id = teamMember.Id,
                FullName = teamMember.FullName,
                Title = teamMember.Title,
                Description = teamMember.Description,
                ImageUrl = teamMember.ImageUrl
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeamMember(UpdateTeamMemberDto updateTeamMemberDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateTeamMemberDto);
            }

            await _teamMemberService.UpdateAsync(updateTeamMemberDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            await _teamMemberService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
