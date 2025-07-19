using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Globalization;

using TicketManager.Services.Contracts;
using TicketManager.Web.Controllers;
using TicketManager.Web.ViewModels.Event;
using static GCommon.GlobalValidation;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Web.Areas.Admin.Controllers
{
    [Area(RoleConstants.Admin)]
    [Authorize(Roles = RoleConstants.Admin)]
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
        public async Task<IActionResult> Create()
        {
            try
            {
                EventCreateInputModel addEventInputModel = new EventCreateInputModel()
                {
                    CreatedOn = DateTime.UtcNow.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    Categories = await _ICategoryService.GetCategoriesDropDownAsync(),
                };

                return this.View(addEventInputModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventCreateInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    inputModel.Categories = await _ICategoryService.GetCategoriesDropDownAsync();
                    return this.View(inputModel);
                }
                bool addResult = await this._EventService.CreateEventAsync(this.GetUserId(), inputModel);

                if (addResult == false)
                {
                    ModelState.AddModelError(string.Empty, "Fatal error occurred while Creating an Event");
                    inputModel.Categories = await _ICategoryService.GetCategoriesDropDownAsync();
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string? userId = this.GetUserId();
                EventDeleteInputModel? deleteEventInputModel = await this._EventService.GetEventForDeletingAsync(userId, id);

                if (deleteEventInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(deleteEventInputModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(EventDeleteInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Please do not modify the page!");
                    return this.View("Delete", inputModel);
                }
                bool deleteResult = await this._EventService.SoftDeleteEventAsync(this.GetUserId()!, inputModel);

                if (deleteResult == false)
                {
                    ModelState.AddModelError(string.Empty, "Fatal error occurred while deleting the Event!");
                    return this.View("Delete", inputModel);
                }
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;
                EventEditInputModel? editEventInputModel = await this._EventService.GetEventForEditingAsync(userId, id);
                if (editEventInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                var categories = await _ICategoryService.GetCategoriesDropDownAsync();

                editEventInputModel.Categories = categories;

                return this.View(editEventInputModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventEditInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }
                bool editResult = await this._EventService.PersistUpdatedEventAsync(this.GetUserId()!, inputModel);

                if (editResult == false)
                {
                    this.ModelState.AddModelError(string.Empty, "Fatal error occurred while updating the Event!");
                    return this.View(inputModel);
                }
                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
