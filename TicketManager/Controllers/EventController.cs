using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TicketManager.Services.Contracts;

using TicketManager.Web.ViewModels.Event;
using static GCommon.GlobalValidation;


namespace TicketManager.Web.Controllers
{
    [Authorize(Roles = RoleConstants.User)]
    public class EventController : BaseController
    {
     
        private readonly ICategoryService _ICategoryService;
        private readonly IEventService _EventService;
       
        public EventController(IEventService eventService, ICategoryService categoryService)
        {
            _EventService = eventService;
             _ICategoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _ICategoryService.GetCategoriesDropDownAsync();

            try
            {
                string? userId = this.User.Identity?.IsAuthenticated == true ? this.GetUserId() : null;
                IEnumerable<EventIndexViewModel> allEvents = await this._EventService.GetAllAsync(userId);
                return this.View(allEvents.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }

        } 

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                string? userId = this.GetUserId();
                EventDetailsViewModel eventDetails = await this._EventService.GetEventDetailsAsync(userId, id);
                if (eventDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(eventDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDetailsPartial(int? id)
        {
            try
            {
                string? userId = this.GetUserId();
                EventDetailsViewModel eventDetails = await this._EventService.GetEventDetailsAsync(userId, id);

                if (eventDetails == null)
                {
                    return NotFound();
                }
                return PartialView("EventDetailsPartial", eventDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            try
            {
                string? userId = this.GetUserId();
                IEnumerable<EventFavoriteViewModel> favoriteEvents = await this._EventService.GetFavoriteEventAsync(userId);
                if (favoriteEvents == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(favoriteEvents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;

                if (id == null)
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }

                bool favAddResult = await this._EventService.AddEventToUserFavoritesListAsync(userId, id.Value);

                if (!favAddResult)
                {
                    return Json(new { success = false, message = "Already added" });
                }

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = "Exception: " + e.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;

                if (id == null)
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }

                bool favRemoveResult = await this._EventService.RemoveEventFromUserFavoritesListAsync(userId, id.Value);

                if (!favRemoveResult)
                {
                    return Json(new { success = false, message = "Failed to remove from favorites" });
                }

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = "Server error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;

                if (id == null)
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }


                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = "Server error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            string? userId = this.GetUserId();
            var events = await this._EventService.SearchEventsAsync(term, userId);

            return PartialView("EventListPartial", events);
        }

    }
}
