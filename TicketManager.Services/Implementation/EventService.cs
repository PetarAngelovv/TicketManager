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
        var events = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.UsersEvents)
            .Include(e => e.Tickets)
            .AsNoTracking()
            .ToListAsync();

        var eventViewModels = events.Select(e => new EventIndexViewModel()
        {
            Id = e.Id,
            Name = e.Name,
            ImageUrl = e.ImageUrl,
            CategoryName = e.Category.Name,
            SavedCount = e.UsersEvents.Count(),
            IsAuthor = userId != null && e.AuthorId.ToLower() == userId.ToLower(),
            IsSaved = userId != null && e.UsersEvents.Any(ur => ur.UserId.ToLower() == userId.ToLower()),
            TicketsLeft = e.Tickets.Count(t => !t.IsSold)
        });

        return eventViewModels;
    }
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

          
            var tickets = new List<Ticket>();
            for (int i = 0; i < newEvent.TotalTickets; i++)
            {
                tickets.Add(new Ticket
                {
                    Price = newEvent.TicketPrice, 
                    EventId = newEvent.Id,
                    IsSold = false
                });
            }

            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync(); 

            opResult = true;
        }

        return opResult;
    }
    public async Task<int> GetTicketsLeftAsync(int eventId)
    {
        return await _context.Tickets
            .Where(t => t.EventId == eventId && !t.IsSold)
            .CountAsync();
    }
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
                    TicketsLeft = _event.Tickets.Count(t => !t.IsSold),
                    Author = _event.Author.UserName
                };

            }
        }
        return detailsEventVm;
    }
    public async Task<EventDeleteInputModel?> GetEventForDeletingAsync(string? userId, int id)
    {
        bool opResult = false;

        EventDeleteInputModel? deleteModel = null;

        if (id != null)
        {
            Event? deleteEventModel = await _context
                .Events
                .Include(r => r.Author)
                 .Include(r => r.Category)
               .AsNoTracking()
               .SingleOrDefaultAsync(r => r.Id == id);

            if (deleteEventModel != null && userId != null)
            {
                IdentityUser? user = await _userManager.FindByIdAsync(userId);
                bool isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

                if (isAdmin || deleteEventModel.AuthorId.ToLower() == userId.ToLower())
                {
                    deleteModel = new EventDeleteInputModel()
                    {
                        Id = deleteEventModel.Id,
                        Name = deleteEventModel.Name,
                        Description = deleteEventModel.Description,
                        CategoryName = deleteEventModel.Category.Name,
                        TicketPrice = deleteEventModel.TicketPrice,
                        TotalTickets = deleteEventModel.TotalTickets,
                        Author = deleteEventModel.Author.UserName,
                        AuthorId = deleteEventModel.AuthorId
                    };
                }
            }
        }
        return deleteModel;
    }
    public async Task<bool> SoftDeleteEventAsync(string userId, EventDeleteInputModel inputModel)
    {

        bool opResult = false;

        IdentityUser? user = await this._userManager.FindByIdAsync(userId);
        Event? _event = await _context.Events.FindAsync(inputModel.Id);

        if (user != null && _event != null)
        {
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin || _event.AuthorId.ToLower() == userId.ToLower())
            {
                _event.IsDeleted = true;
                await this._context.SaveChangesAsync();
                opResult = true;
            }
        }

        return opResult;
    }
    public async Task<bool> HardDeleteEventAsync(string userId, int eventId)
    {
        var eventEntity = await _context.Events.FindAsync(eventId);
        if (eventEntity == null) return false;

        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync();
        return true;
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
    }
    public async Task<bool> PersistUpdatedEventAsync(string userId, EventEditInputModel inputModel)
 {
     if (string.IsNullOrWhiteSpace(userId) || inputModel == null)
         return false;

     var user = await _userManager.FindByIdAsync(userId);
     if (user == null)
         return false;

     var ev = await _context.Events.FindAsync(inputModel.Id);
     if (ev == null || !string.Equals(ev.AuthorId, userId, StringComparison.OrdinalIgnoreCase))
         return false;

     var category = await _context.Categories.FindAsync(inputModel.CategoryId);
     if (category == null)
         return false;

     if (!DateTime.TryParseExact(
             inputModel.CreatedOn,
             CreatedOnFormat,
             CultureInfo.InvariantCulture,
             DateTimeStyles.None,
             out DateTime createdOnParsed))
             return false;

         ev.Name = inputModel.Name;
         ev.Description = inputModel.Description;
         ev.TicketPrice = inputModel.TicketPrice;
         ev.TotalTickets = inputModel.TotalTickets;
         ev.ImageUrl = inputModel.ImageUrl;
         ev.CategoryId = category.Id;
         ev.Category = category;
         ev.CreatedOn = createdOnParsed;

         await _context.SaveChangesAsync();
         return true;
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
                    .ThenInclude(e => e.Category)
                .Include(ur => ur.Event.Tickets)
                .AsNoTracking()
                .Where(ur => ur.UserId.ToLower() == userId.ToLower())
                .Select(ur => new EventFavoriteViewModel()
                {
                    Id = ur.Event.Id,
                    Name = ur.Event.Name,
                    ImageUrl = ur.Event.ImageUrl,
                    Category = ur.Event.Category.Name,
                    TicketsLeft = ur.Event.Tickets.Count(t => !t.IsSold) 
                })
                .ToArrayAsync();
        }

        return favEvents;
    }
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
    }
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
    }
    public async Task<IEnumerable<EventIndexViewModel>> GetAllByCreatorAsync(string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Enumerable.Empty<EventIndexViewModel>();
        }

        return await _context.Events
            .Where(e => e.AuthorId == userId && !e.IsDeleted)
            .Select(e => new EventIndexViewModel
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = e.ImageUrl,
                IsAuthor = true,
                CategoryName = e.Category.Name,
                SavedCount = 0
            })
            .ToListAsync();
    }
    public async Task<IEnumerable<EventIndexViewModel>> SearchEventsAsync(string? term, int? categoryId, string? userId)
    {
        var query = this._context.Events
            .Where(e => !e.IsDeleted);

        if (!string.IsNullOrWhiteSpace(term))
        {
            string lowered = term.ToLower();
            query = query.Where(e =>
                e.Name.ToLower().Contains(lowered) ||
                e.Description.ToLower().Contains(lowered));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        var results = await query
            .Select(e => new EventIndexViewModel
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = e.ImageUrl,
                IsAuthor = e.AuthorId == userId,
                CategoryName = e.Category.Name,
                IsSaved = e.UsersEvents.Any(ue => ue.UserId == userId)
            })
            .ToListAsync();

        return results;
    }
    public async Task BuyTicketAsync(int eventId, string userId)
    {
        var availableTicket = await _context.Tickets
            .Where(t => t.EventId == eventId && !t.IsSold)
            .FirstOrDefaultAsync();

        if (availableTicket == null)
        {
            throw new InvalidOperationException("No available tickets for this event.");
        }

    
        var order = new Order
        {
            UserId = userId,
            PurchaseDate = DateTime.UtcNow,
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

       
        var orderTicket = new OrderTicket
        {
            OrderId = order.Id,
            TicketId = availableTicket.Id
        };

        await _context.OrderTickets.AddAsync(orderTicket);

        
        availableTicket.IsSold = true;

        await _context.SaveChangesAsync();
    }
}