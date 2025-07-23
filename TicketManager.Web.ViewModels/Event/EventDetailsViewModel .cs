namespace TicketManager.Web.ViewModels.Event
{
    public class EventDetailsViewModel : BaseEventInputModel
    {
        public string Description { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;
        public string Author { get; set; } = null!;
        public decimal TicketPrice { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsLeft { get; set; }
    }
}