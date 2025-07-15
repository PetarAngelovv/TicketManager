using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManager.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public virtual IdentityUser User { get; set; } = null!;

        public virtual DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public virtual IEnumerable<OrderTicket> OrderTickets { get; set; } = new HashSet<OrderTicket>();
    }
}
