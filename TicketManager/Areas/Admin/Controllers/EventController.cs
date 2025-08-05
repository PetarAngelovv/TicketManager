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

        private readonly ICategoryService _categoryService;
        private readonly IEventService _eventService;

        public EventController(IEventService eventService, ICategoryService categoryService)
        {
            _eventService = eventService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var allEvents = await _eventService.GetAllAsync(null, isAdmin: true);

                var allEventsViewModels = allEvents
                    .Select(e => new EventIndexViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        IsDeleted = e.IsDeleted,
                        CategoryName = e.CategoryName ?? "Unknown"
                    })
                    .ToList();

                return View(allEventsViewModels);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index), "Home");
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
                    Categories = await _categoryService.GetCategoriesDropDownAsync(),
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
                    inputModel.Categories = await _categoryService.GetCategoriesDropDownAsync();
                    return this.View(inputModel);
                }
                bool addResult = await this._eventService.CreateEventAsync(this.GetUserId(), inputModel);

                if (addResult == false)
                {
                    ModelState.AddModelError(string.Empty, "Fatal error occurred while Creating an Event");
                    inputModel.Categories = await _categoryService.GetCategoriesDropDownAsync();
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
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                string userId = this.GetUserId();

                var eventDetails = await _eventService.GetEventDetailsAsync(userId, id);

                if (eventDetails == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(eventDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Details: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string? userId = this.GetUserId();
                EventDeleteInputModel? deleteEventInputModel = await this._eventService.GetEventForDeletingAsync(userId, id);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(EventDeleteInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Please do not modify the page!");
                    return this.View("Delete", inputModel);
                }
                bool deleteResult = await this._eventService.SoftDeleteEventAsync(this.GetUserId()!, inputModel);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                if (!User.IsInRole("Admin"))
                {
                    TempData["ErrorMessage"] = "Unauthorized: Only Admins can hard delete events.";
                    return RedirectToAction("Details", new { id });
                }

                bool result = await _eventService.HardDeleteEventAsync(GetUserId(), id);

                if (!result)
                {
                    TempData["ErrorMessage"] = "Hard deletion failed or event not found.";
                    return RedirectToAction("Details", new { id });
                }

                TempData["SuccessMessage"] = "Event permanently deleted.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HardDelete: {ex.Message}");
                TempData["ErrorMessage"] = "Unexpected error occurred while deleting event.";
                return RedirectToAction("Details", new { id });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                string userId = this.GetUserId()!;
                bool isAdmin = User.IsInRole("Admin");
                EventEditInputModel? editEventInputModel = await this._eventService.GetEventForEditingAsync(userId, id, isAdmin);

                if (editEventInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                var categories = await _categoryService.GetCategoriesDropDownAsync();

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

                bool isAdmin = User.IsInRole("Admin");
                bool editResult = await this._eventService.PersistUpdatedEventAsync(this.GetUserId()!, inputModel, isAdmin);

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
