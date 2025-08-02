namespace TicketManager.Web.ViewModels.Event
{
    public class EventIndexViewModel : BaseEventInputModel
    {
        public int SavedCount { get; set; }
        public int TicketsLeft { get; set; }
        public bool IsDeleted { get; set; }
    }
}
