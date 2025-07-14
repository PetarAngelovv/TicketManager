using Microsoft.AspNetCore.Mvc;

namespace TicketManager.Web.Controllers
{
    public class LegalController : Controller
    {
        [HttpGet("/delete-data")]
        public IActionResult DeleteData()
        {
            return View();
        }
    }
}
