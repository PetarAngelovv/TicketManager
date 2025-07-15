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
    public EventService(TicketManagerDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
        EventDetailsViewModel? detailsEventVm = null;

        if (id.HasValue)
        {
            Event? _event = await _context.Events
                .Include(r => r.Category)
                .Include(r => r.UsersEvents)
                .Include(r => r.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id.Value);
            if (_event != null)
            {
                detailsEventVm = new EventDetailsViewModel()
                {
                    Id = _event.Id,
                    Name = _event.Name,
                    ImageUrl = _event.ImageUrl,
                    CategoryName = _event.Category.Name,
                    IsAuthor = userId != null ? _event.AuthorId.ToLower() == userId.ToLower() : false,
                    IsSaved = _event.UsersEvents.Any(ur => userId != null ? ur.UserId.ToLower() == userId.ToLower() : false),
                    Description = _event.Description,
                    CreatedOn = _event.CreatedOn.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    TicketPrice = _event.TicketPrice,
                    TotalTickets = _event.TotalTickets,
                    Author = _event.Author.UserName
                };

            }
        }
        return detailsEventVm;
    }//DONE
    public async Task<EventDeleteInputModel?> GetEventForDeletingAsync(string? userId, int id)
    {
        bool opResult = false;

        EventDeleteInputModel? deleteModel = null;

        if (id != null)
        {
            Event? DeleteEventModel = await _context
                .Events
                .Include(r => r.Author)
                 .Include(r => r.Category)
               .AsNoTracking()
               .SingleOrDefaultAsync(r => r.Id == id);

            if (DeleteEventModel != null && DeleteEventModel.AuthorId.ToLower() == userId.ToLower())
            {
                deleteModel = new EventDeleteInputModel()
                {
                    Id = DeleteEventModel.Id,
                    Name = DeleteEventModel.Name,
                    Description = DeleteEventModel.Description,
                    CategoryName = DeleteEventModel.Category.Name,
                    TicketPrice = DeleteEventModel.TicketPrice,
                    TotalTickets = DeleteEventModel.TotalTickets,
                    Author = DeleteEventModel.Author.UserName,
                    AuthorId = DeleteEventModel.AuthorId
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
            Event? eventToEdit = await this._context.Events
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == id);

            if (eventToEdit != null && eventToEdit.AuthorId.ToLower() == userId.ToLower())
            {
                editModel = new EventEditInputModel()
                {
                    Id = eventToEdit.Id,
                    Name = eventToEdit.Name,
                    Description = eventToEdit.Description,
                    ImageUrl = eventToEdit.ImageUrl,
                    CreatedOn = eventToEdit.CreatedOn.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    CategoryId = eventToEdit.CategoryId, 
                    TotalTickets = eventToEdit.TotalTickets,
                    TicketPrice = eventToEdit.TicketPrice
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

        Event? favEvent = await this._context
            .Events
            .FindAsync(id);

        if (user != null && favEvent != null &&
            favEvent.AuthorId.ToLower() != userId.ToLower())
        {
            UserEvent? userFavEvent = await this._context
                .UsersEvents
                .SingleOrDefaultAsync(ud => ud.UserId.ToLower() == userId && ud.EventId == id);

            if (userFavEvent == null)
            {
                userFavEvent = new UserEvent()
                {
                    UserId = userId,
                    EventId = id
                };

                await this._context.UsersEvents.AddAsync(userFavEvent);
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
            UserEvent? userFavEvent = await this._context
                .UsersEvents
                .SingleOrDefaultAsync(ud => ud.UserId.ToLower() == userId.ToLower() && ud.EventId == id);

            if (userFavEvent != null)
            {
                this._context.UsersEvents.Remove(userFavEvent);
                await this._context.SaveChangesAsync();

                opResult = true;
            }
        }

        return opResult;
    }//DONE
}