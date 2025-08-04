using GCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManager.Web.Models;
using TicketManager.Web.ViewModels.User;
using static GCommon.GlobalValidation;

namespace TicketManager.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagementController(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var users = _userManager.Users
                                   .Where(u => u.Id != currentUserId)
                                   .ToList();

            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(RoleConstants.Admin))
                {
                    continue;
                }

                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    IsManager = roles.Contains(RoleConstants.Manager)
                });
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddManagerRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !await _userManager.IsInRoleAsync(user, RoleConstants.Manager))
            {

                await _userManager.AddToRoleAsync(user, RoleConstants.Manager);
            }
            return RedirectToAction("Index", "UserManagement", new { area = "Admin" });

        }

        [HttpPost]
        public async Task<IActionResult> RemoveManagerRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _userManager.IsInRoleAsync(user, RoleConstants.Manager))
            {

                await _userManager.RemoveFromRoleAsync(user, RoleConstants.Manager);
            }
            return RedirectToAction("Index", "UserManagement", new { area = "Admin" });

        }

        public IActionResult DeleteRequests()
        {
            var pendingRequests = DeleteRequestStore.RequestedUsers ?? Enumerable.Empty<string>();
            return View(pendingRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveDeletion(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                DeleteRequestStore.RemoveRequest(username);
            }
            return RedirectToAction("DeleteRequests");
        }

        [HttpPost]
        public IActionResult RejectDeletion(string username)
        {
            DeleteRequestStore.RemoveRequest(username);
            return RedirectToAction("DeleteRequests");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    DeleteRequestStore.RemoveRequest(username);
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
