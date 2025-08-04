using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManager.Web.Models;

namespace TicketManager.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult RequestDelete()
        {
            var username = User.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                DeleteRequestStore.AddRequest(username);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "User not found" });
        }

        [HttpPost]
        public IActionResult CancelDelete()
        {
            var username = User.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                DeleteRequestStore.RemoveRequest(username);
            }
              return Json(new { success = false, message = "User not found" });
        }
    }
}
