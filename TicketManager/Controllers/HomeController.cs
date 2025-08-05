using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketManager.Models;
using TicketManager.Web.Controllers;
using static GCommon.GlobalValidation;
namespace TicketManager.Controllers
{
    [Authorize(Roles = RoleConstants.User)]
    public class HomeController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
                {
                    return RedirectToAction("Index", "Event", new { area = "Admin" });
                }

                if (await _userManager.IsInRoleAsync(currentUser, "Manager"))
                {
                    return RedirectToAction("Index", "MyEvent", new { area = "Manager" });
                }

                if (await _userManager.IsInRoleAsync(currentUser, "User"))
                {
                    return RedirectToAction("Index", "Event");
                }
            }

            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
