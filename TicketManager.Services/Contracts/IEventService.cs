using Microsoft.EntityFrameworkCore;
using TicketManager.Data.Models;
using TicketManager.Web.ViewModels.Event;

namespace TicketManager.Services.Contracts
{
    public interface IEventService
    {
            Task<IEnumerable<EventIndexViewModel>> GetAllAsync(string? userId);
            Task<bool> CreateEventAsync(string? userId, EventCreateInputModel inputModel);
            Task<EventDetailsViewModel?> GetEventDetailsAsync(string userId, int? id);
            Task<EventDeleteInputModel?> GetEventForDeletingAsync(string? userId, int id);
            Task<bool> SoftDeleteEventAsync(string userId, EventDeleteInputModel inputModel);
            Task<bool> HardDeleteEventAsync(string userId, int eventId);
            Task<EventEditInputModel?> GetEventForEditingAsync(string userId, int? id);
            Task<bool> PersistUpdatedEventAsync(string userId, EventEditInputModel inputModel);
            Task<IEnumerable<EventFavoriteViewModel>> GetFavoriteEventAsync(string userId);
            Task<bool> AddEventToUserFavoritesListAsync(string userId, int id);
            Task<bool> RemoveEventFromUserFavoritesListAsync(string userId, int id);
            Task<IEnumerable<EventIndexViewModel>> GetAllByCreatorAsync(string? userId);
            Task<IEnumerable<EventIndexViewModel>> SearchEventsAsync(string term, int? categoryId, string? userId);
            Task BuyTicketAsync(int eventId, string userId);
            Task<int> GetTicketsLeftAsync(int eventId);
            Task<List<Event>> GetAllEventsForAdminAsync();

    }
}
