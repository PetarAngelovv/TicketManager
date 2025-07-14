using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TicketManager.Data;
using TicketManager.Data.Models;
using TicketManager.Services.Contracts;
using TicketManager.Web.ViewModels.Event;
using static GCommon.GlobalValidation.Event;

public class EventService : IEventService
{
    private readonly TicketManagerDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public EventService(TicketManagerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventIndexViewModel>> GetAllAsync(string? userId)
    {
        IEnumerable<EventIndexViewModel> events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.UsersEvents)
                .AsNoTracking()
                .Select(e => new EventIndexViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    ImageUrl = e.ImageUrl,
                    CategoryName = e.Category.Name,
                    SavedCount = e.UsersEvents.Count(),
                    IsAuthor = userId != null ? e.AuthorId.ToLower() == userId.ToLower() : false,
                    IsSaved = userId != null ? e.UsersEvents.Any(ur => ur.UserId.ToLower() == userId.ToLower()) : false

                })
               .ToArrayAsync();

        return events;
    }  //DONE

    public async Task<bool> CreateEventAsync(string? userId, EventCreateInputModel inputModel)
    {
        bool opResult = false;
        IdentityUser? user = await this._userManager.FindByIdAsync(userId);
        Category? category = await _context.Categories.FindAsync(inputModel.CategoryId);
        bool isCreatedOnValid = DateTime.TryParseExact(inputModel.CreatedOn, CreatedOnFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdOn);

        if (user != null && category != null && isCreatedOnValid)
        {
            Event newEvent = new Event
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                CreatedOn = createdOn,
                TicketPrice = inputModel.TicketPrice,
                TotalTickets = inputModel.TotalTickets,
                ImageUrl = inputModel.ImageUrl,
                AuthorId = user.Id,
                Author = user,
                CategoryId = category.Id,
                Category = category

            };
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
        }

        opResult = true;

         return opResult;
    }//DONE

    public async Task<EventDetailsViewModel> GetEventDetailsAsync(string userId, int? id)
    {
        EventDetailsViewModel? detailsRecipeVm = null;

        if (id.HasValue)
        {
            Event? recipe = await _context.Events
                .Include(r => r.Category)
                .Include(r => r.UsersEvents)
                .Include(r => r.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id.Value);
            if (recipe != null)
            {
                detailsRecipeVm = new EventDetailsViewModel()
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    ImageUrl = recipe.ImageUrl,
                    CategoryName = recipe.Category.Name,
                    IsAuthor = userId != null ? recipe.AuthorId.ToLower() == userId.ToLower() : false,
                    IsSaved = recipe.UsersEvents.Any(ur => userId != null ? ur.UserId.ToLower() == userId.ToLower() : false),
                    Description = recipe.Description,
                    CreatedOn = recipe.CreatedOn.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    Author = recipe.Author.UserName
                };

            }
        }
        return detailsRecipeVm;
    }//DONE

    public async Task<EventDeleteInputModel?> GetEventForDeletingAsync(string? userId, int id)
    {
        bool opResult = false;

        EventDeleteInputModel? deleteModel = null;

        if (id != null)
        {
            Event? DeleteRecipeModel = await _context
                .Events
                .Include(r => r.Author)
               .AsNoTracking()
               .SingleOrDefaultAsync(r => r.Id == id);

            if (DeleteRecipeModel != null && DeleteRecipeModel.AuthorId.ToLower() == userId.ToLower())
            {
                deleteModel = new EventDeleteInputModel()
                {
                    Id = DeleteRecipeModel.Id,
                    Name = DeleteRecipeModel.Name,
                    Author = DeleteRecipeModel.Author.UserName,
                    AuthorId = DeleteRecipeModel.AuthorId
                };
            }
        }
        return deleteModel;
    }//DONE

    public async Task<bool> SoftDeleteEventAsync(string userId, EventDeleteInputModel inputModel) //DONE    
    {

        bool opResult = false;

        IdentityUser? user = await this._userManager.FindByIdAsync(userId);

        Event? _event = await _context
            .Events.FindAsync(inputModel.Id);

        if (user != null && _event != null && _event.AuthorId.ToLower() == userId.ToLower())
        {
            _event.IsDeleted = true;

            await this._context.SaveChangesAsync();
            opResult = true;
        }

        return opResult;
    }

    public async Task<EventEditInputModel?> GetEventForEditingAsync(string userId, int? id)
    {
        EventEditInputModel? editModel = null;

        if (id != null)
        {
            Event? recipeToEdit = await this._context.Events
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == id);

            if (recipeToEdit != null && recipeToEdit.AuthorId.ToLower() == userId.ToLower())
            {
                editModel = new EventEditInputModel()
                {
                    Id = recipeToEdit.Id,
                    Name = recipeToEdit.Name,
                    Description = recipeToEdit.Description,
                    ImageUrl = recipeToEdit.ImageUrl,
                    CreatedOn = recipeToEdit.CreatedOn.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    CategoryId = recipeToEdit.CategoryId,
                    Categories = await _context.Categories
                        .Select(c => new AddCategoryDropDownModel()
                        {
                            Id = c.Id,
                            Name = c.Name
                        })
                        .ToListAsync()
                };
            }

        }
        return editModel;
    } //  DONE

    public async Task<bool> PersistUpdatedEventAsync(string userId, EventEditInputModel inputModel)//  DONE
    {
        bool opResult = false;
        IdentityUser? user = await this._userManager.FindByIdAsync(userId);
        var ev = await _context.Events
            .FindAsync(inputModel.Id);

        Category? category = await _context.Categories.FindAsync(inputModel.CategoryId);

        bool isDateValid = DateTime.TryParseExact(inputModel.CreatedOn, CreatedOnFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdOnParsed);

        if (user != null && category != null && ev != null && isDateValid && ev.AuthorId.ToLower() == userId.ToLower())
        {
            ev.Name = inputModel.Name;
            ev.Description = inputModel.Description;
            ev.TicketPrice = inputModel.TicketPrice;
            ev.TotalTickets = inputModel.TotalTickets;
            ev.ImageUrl = inputModel.ImageUrl;
            ev.CategoryId = category.Id;
            ev.Category = category;
            ev.CreatedOn = createdOnParsed;
        }

        await this._context.SaveChangesAsync();

        opResult = true;

        return opResult;

    }

    public async Task<IEnumerable<EventFavoriteViewModel>> GetFavoriteEventAsync(string userId)
    {
        IEnumerable<EventFavoriteViewModel>? favEvents = null;
        IdentityUser? user = await this._userManager.FindByIdAsync(userId);

        if (user != null)
        {
            favEvents = await this._context
                .UsersEvents
                .Include(ur => ur.Event)
                .ThenInclude(ur => ur.Category)
                .AsNoTracking()
                 .Where(ur => ur.UserId.ToLower() == userId.ToLower())
                .Select(ur => new EventFavoriteViewModel()
                {
                    Id = ur.Event.Id,
                    Name = ur.Event.Name,
                    ImageUrl = ur.Event.ImageUrl,
                    Category = ur.Event.Category.Name


                })
                .ToArrayAsync();
        }
        return favEvents;
    }//DONE

    public async Task<bool> AddEventToUserFavoritesListAsync(string userId, int id)
    {
        bool opResult = false;

        IdentityUser? user = await this._userManager.FindByIdAsync(userId);

        Event? favRecipe = await this._context
            .Events
            .FindAsync(id);

        if (user != null && favRecipe != null &&
            favRecipe.AuthorId.ToLower() != userId.ToLower())
        {
            UserEvent? userFavRecipe = await this._context
                .UsersEvents
                .SingleOrDefaultAsync(ud => ud.UserId.ToLower() == userId && ud.EventId == id);

            if (userFavRecipe == null)
            {
                userFavRecipe = new UserEvent()
                {
                    UserId = userId,
                    EventId = id
                };

                await this._context.UsersEvents.AddAsync(userFavRecipe);
                await this._context.SaveChangesAsync();

                opResult = true;
            }
        }

        return opResult;
    }//DONE

    public async Task<bool> RemoveEventFromUserFavoritesListAsync(string userId, int id)
    {
        bool opResult = false;

        IdentityUser? user = await this._userManager.FindByIdAsync(userId);

        if (user != null)
        {
            UserEvent? userFavRecipe = await this._context
                .UsersEvents
                .SingleOrDefaultAsync(ud => ud.UserId.ToLower() == userId.ToLower() && ud.EventId == id);

            if (userFavRecipe != null)
            {
                this._context.UsersEvents.Remove(userFavRecipe);
                await this._context.SaveChangesAsync();

                opResult = true;
            }
        }

        return opResult;
    }//DONE
}