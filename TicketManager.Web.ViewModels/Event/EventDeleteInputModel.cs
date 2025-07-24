namespace TicketManager.Web.ViewModels.Event
{
    public class EventDeleteInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal TicketPrice { get; set; }
        public int TotalTickets { get; set; }
        public string? Author { get; set; }
        public string AuthorId { get; set; } = null!;
    }
}