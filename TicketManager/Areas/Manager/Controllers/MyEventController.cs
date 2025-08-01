using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TicketManager.Services.Contracts;
using TicketManager.Web.Controllers;
using TicketManager.Web.ViewModels.Event;
using static GCommon.GlobalValidation;

namespace TicketManager.Web.Areas.Manager.Controllers
{
    [Area(RoleConstants.Manager)]
    [Authorize(Roles = RoleConstants.Manager)]
    public class MyEventController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IEventService _eventService;

        public MyEventController(IEventService eventService, ICategoryService categoryService)
        {
            _eventService = eventService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();
            var events = await _eventService.GetAllAsync(userId);

            var myEvents = events
                .Where(e => e.IsAuthor) 
                .ToList();

            return View(myEvents);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EventCreateInputModel
            {
                CreatedOn = DateTime.UtcNow.ToString(Event.CreatedOnFormat, CultureInfo.InvariantCulture),
                Categories = await _categoryService.GetCategoriesDropDownAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                inputModel.Categories = await _categoryService.GetCategoriesDropDownAsync();
                return View(inputModel);
            }

            bool result = await _eventService.CreateEventAsync(GetUserId(), inputModel);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error creating event");
                inputModel.Categories = await _categoryService.GetCategoriesDropDownAsync();
                return View(inputModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _eventService.GetEventDetailsAsync(GetUserId(), id);
            if (model == null)
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _eventService.GetEventForEditingAsync(GetUserId(), id);
            if (model == null)
                return RedirectToAction(nameof(Index));

            model.Categories = await _categoryService.GetCategoriesDropDownAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventEditInputModel inputModel)
        {
            if (!ModelState.IsValid)
                return View(inputModel);

            bool result = await _eventService.PersistUpdatedEventAsync(GetUserId(), inputModel);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error updating event");
                return View(inputModel);
            }

            return RedirectToAction(nameof(Details), new { id = inputModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _eventService.GetEventForDeletingAsync(GetUserId(), id);
            if (model == null)
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(EventDeleteInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Do not modify the page!");
                return View("Delete", inputModel);
            }

            bool result = await _eventService.SoftDeleteEventAsync(GetUserId(), inputModel);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error deleting event");
                return View("Delete", inputModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            bool result = await _eventService.HardDeleteEventAsync(GetUserId(), id);

            if (!result)
            {
                TempData["ErrorMessage"] = "Hard deletion failed.";
                return RedirectToAction("Details", new { id });
            }

            TempData["SuccessMessage"] = "Event permanently deleted.";
            return RedirectToAction("Index");
        }

    }
}
