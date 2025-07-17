using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketManager.Models;
using TicketManager.Web.Controllers;
using static GCommon.GlobalValidation;
namespace TicketManager.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Event", new { area = "Admin" });
                }
                else if (await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    return RedirectToAction("Index", "MyEvent", new { area = "Manager" });

                }
                else
                {
                    return RedirectToAction("Index", "Event");
                }
                 
            }
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
