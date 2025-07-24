namespace TicketManager.Web.ViewModels.Event
{
    public class BaseEventInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsAuthor { get; set; }
        public string CategoryName { get; set; } = null!;
        public bool IsSaved { get; set; }
    }
}