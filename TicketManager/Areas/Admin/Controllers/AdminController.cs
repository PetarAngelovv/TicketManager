using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static GCommon.GlobalValidation;

namespace TicketManager.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddManagerRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null && !await userManager.IsInRoleAsync(user, RoleConstants.Manager))
            {
                // Добавя Manager роля на потребителя
                await userManager.AddToRoleAsync(user, RoleConstants.Manager);
            }
            return RedirectToAction("Index", "UserManagement", new { area = "Admin" });

        }
        [HttpPost]
        public async Task<IActionResult> RemoveManagerRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null && await userManager.IsInRoleAsync(user, RoleConstants.Manager))
            {
                // Премахва Manager роля от потребителя
                await userManager.RemoveFromRoleAsync(user, RoleConstants.Manager);
            }
            return RedirectToAction("Index", "UserManagement", new { area = "Admin" });

        }
    }

}
