using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;

using TicketManager.Services.Contracts;
using TicketManager.Web.Controllers;
using TicketManager.Web.ViewModels.Event;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Web.Controllers
{

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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = this.GetUserId();
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
        [AllowAnonymous]
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
        public async Task<IActionResult> Save(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;

                if (id == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                bool favAddResult = await this._EventService.AddEventToUserFavoritesListAsync(userId, id.Value);

                if (favAddResult == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Favorites));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
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
                    return this.RedirectToAction(nameof(Index));
                }

                bool favRemoveResult = await this._EventService.RemoveEventFromUserFavoritesListAsync(userId, id.Value);

                if (favRemoveResult == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Favorites));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
