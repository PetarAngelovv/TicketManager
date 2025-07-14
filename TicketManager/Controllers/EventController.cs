using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

using TicketManager.Services.Contracts;
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
                IEnumerable<EventIndexViewModel> allRecipes = await this._EventService.GetAllAsync(userId);
                return this.View(allRecipes);
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
                EventCreateInputModel addRecipeInputModel = new EventCreateInputModel()
                {
                    CreatedOn = DateTime.UtcNow.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    Categories = await _ICategoryService.GetCategoriesDropDownAsync(),
                };

                return this.View(addRecipeInputModel);
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
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                string? userId = this.GetUserId();
                EventDetailsViewModel recipeDetails = await this._EventService.GetEventDetailsAsync(userId, id);
                if (recipeDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(recipeDetails);
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
                EventDeleteInputModel? deleteRecipeInputModel = await this._EventService.GetEventForDeletingAsync(userId, id);
                if (deleteRecipeInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(deleteRecipeInputModel);
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
                    return this.View(inputModel);
                }
                bool deleteResult = await this._EventService.SoftDeleteEventAsync(this.GetUserId()!, inputModel);

                if (deleteResult == false)
                {
                    ModelState.AddModelError(string.Empty, "Fatal error occurred while deleting the Event!");
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

                editEventInputModel.Categories = await _ICategoryService.GetCategoriesDropDownAsync();

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
