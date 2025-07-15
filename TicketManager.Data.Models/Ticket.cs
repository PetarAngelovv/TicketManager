namespace TicketManager.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; } = false;
        public int EventId { get; set; }
        public virtual Event Event { get; set; } = null!;

        public virtual IEnumerable<OrderTicket> OrderTickets { get; set; } = new HashSet<OrderTicket>();
    }
}
