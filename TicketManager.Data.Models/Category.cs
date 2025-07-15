using TicketManager.Data.Models;
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual IEnumerable<Event> Events { get; set; } = new HashSet<Event>();
}