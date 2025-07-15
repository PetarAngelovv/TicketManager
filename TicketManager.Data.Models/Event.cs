
using Microsoft.AspNetCore.Identity;

namespace TicketManager.Data.Models
{
    public class Event 
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal TicketPrice { get; set; }
        public int TotalTickets { get; set; }
        public string? ImageUrl { get; set; }
        public string AuthorId { get; set; } = null!;
        public virtual IdentityUser Author { get; set; } = null!;
        public virtual DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
        public virtual IEnumerable<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<UserEvent> UsersEvents { get; set; } = new HashSet<UserEvent>();
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}