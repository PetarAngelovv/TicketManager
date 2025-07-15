namespace TicketManager.Data.Models
{
    public class OrderTicket
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; } = null!;
    }
}
