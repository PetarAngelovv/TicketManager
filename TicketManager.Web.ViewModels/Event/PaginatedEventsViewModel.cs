namespace TicketManager.Web.ViewModels.Event
{
    public class PaginatedEventsViewModel
    {
        public List<EventIndexViewModel> Events { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
    }
}
