using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManager.Services.Contracts;
using TicketManager.Web.ViewModels.Event;
using TicketManager.Web.Models;
using static GCommon.GlobalValidation;


namespace TicketManager.Web.Controllers
{
    [Authorize(Roles = RoleConstants.User)]

    public class EventController : BaseController
    {

        private readonly ICategoryService _categoryService;
        private readonly IEventService _eventService;
        private readonly IOrderService _orderService;
        public EventController(IEventService eventService, ICategoryService categoryService, IOrderService orderService)
        {
            _eventService = eventService;
             _categoryService = categoryService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            ViewBag.Categories = await _categoryService.GetCategoriesDropDownAsync();

            try
            {
                string? userId = this.User.Identity?.IsAuthenticated == true ? this.GetUserId() : null;
                IEnumerable<EventIndexViewModel> allEvents = await this._eventService.GetAllAsync(userId);

                // Paging
                int totalItems = allEvents.Count();
                var pagedEvents = allEvents
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Update TicketsLeft for each event
                foreach (var ev in pagedEvents)
                {
                    ev.TicketsLeft = await _eventService.GetTicketsLeftAsync(ev.Id);
                }

                var viewModel = new PaginatedEventsViewModel
                {
                    Events = pagedEvents,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return this.View(viewModel);
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
                EventDetailsViewModel eventDetails = await this._eventService.GetEventDetailsAsync(userId, id);
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
                EventDetailsViewModel eventDetails = await this._eventService.GetEventDetailsAsync(userId, id);

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
                IEnumerable<EventFavoriteViewModel> favoriteEvents = await this._eventService.GetFavoriteEventAsync(userId);
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
        public async Task<IActionResult> Save(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;

                if (id == null)
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }

                bool favAddResult = await this._eventService.AddEventToUserFavoritesListAsync(userId, id.Value);

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
        public async Task<IActionResult> Remove(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;

                if (id == null)
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }

                bool favRemoveResult = await this._eventService.RemoveEventFromUserFavoritesListAsync(userId, id.Value);

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

        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            try
            {
                string userId = this.GetUserId()!;

                await this._eventService.BuyTicketAsync(id, userId);

                return Json(new { success = true });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTicketsLeft(int id)
        {
            try
            {
                int ticketsLeft = await _eventService.GetTicketsLeftAsync(id);
                return Json(new { success = true, ticketsLeft });
            }
            catch
            {
                return Json(new { success = false, message = "Unable to fetch tickets left." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Search(string term = "", int? categoryId = null, int page = 1, int pageSize = 6)
        {
            try
            {
                string? userId = this.User.Identity?.IsAuthenticated == true ? this.GetUserId() : null;
                IEnumerable<EventIndexViewModel> filteredEvents = await _eventService.SearchEventsAsync(term, categoryId, userId);

                int totalItems = filteredEvents.Count();
                var pagedEvents = filteredEvents
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var ev in pagedEvents)
                {
                    ev.TicketsLeft = await _eventService.GetTicketsLeftAsync(ev.Id);
                }

                var viewModel = new PaginatedEventsViewModel
                {
                    Events = pagedEvents,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return PartialView("EventListPartial", viewModel);
            }
            catch (Exception ex)
            {
                return PartialView("EventListPartial", new PaginatedEventsViewModel());
            }
        }

    }
}
