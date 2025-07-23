using Microsoft.EntityFrameworkCore;
using TicketManager.Data;
using TicketManager.Data.Models;
using TicketManager.Services.Contracts;

public class OrderService : IOrderService
{
    private readonly TicketManagerDbContext _context;

    public OrderService(TicketManagerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> BuyTicketAsync(int eventId, string userId)
    {
        var @event = await _context.Events
            .Include(e => e.Tickets)
            .FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted);

        if (@event == null || @event.TotalTickets < 1)
        {
            return false;
        }


        @event.TotalTickets--;

        var ticket = new Ticket
        {
            EventId = eventId
        };

        var order = new Order
        {
            UserId = userId,
            PurchaseDate = DateTime.UtcNow
        };

        var orderTicket = new OrderTicket
        {
            Ticket = ticket,
            Order = order
        };

        _context.Orders.Add(order);
        _context.OrderTickets.Add(orderTicket);
        await _context.SaveChangesAsync();

        return true;
    }

}
