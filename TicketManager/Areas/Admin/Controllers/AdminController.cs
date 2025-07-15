using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public async Task<IActionResult> AddManagerRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null && !await userManager.IsInRoleAsync(user, "Manager"))
            {
                // Добавя Manager роля на потребителя
                await userManager.AddToRoleAsync(user, "Manager");
            }
            return RedirectToAction("Index", "UserManagement"); // или където искаш
        }

        public async Task<IActionResult> RemoveManagerRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null && await userManager.IsInRoleAsync(user, "Manager"))
            {
                // Премахва Manager роля от потребителя
                await userManager.RemoveFromRoleAsync(user, "Manager");
            }
            return RedirectToAction("Index", "UserManagement");
        }
    }

}
