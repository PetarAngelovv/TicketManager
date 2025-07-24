namespace TicketManager.Web.ViewModels.Event
{
    public class EventFavoriteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = null!;
        public int TicketsLeft { get; set; } 
    }
}
