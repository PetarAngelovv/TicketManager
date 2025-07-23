using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketManager.Services.Contracts;
using static GCommon.GlobalValidation;

namespace TicketManager.Web.Areas.Admin.Controllers
{
    [Area(RoleConstants.Admin)]
    [Authorize(Roles=RoleConstants.Admin)]
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
             
                await userManager.RemoveFromRoleAsync(user, RoleConstants.Manager);
            }
            return RedirectToAction("Index", "UserManagement", new { area = "Admin" });

        }
    }

}
