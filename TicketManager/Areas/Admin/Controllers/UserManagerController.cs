using GCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManager.Web.ViewModels.User;
using static GCommon.GlobalValidation;

namespace TicketManager.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserManagementController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = userManager.GetUserId(User); 
            var users = userManager.Users
                                   .Where(u => u.Id != currentUserId)  
                                   .ToList();

            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

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
    }
}
