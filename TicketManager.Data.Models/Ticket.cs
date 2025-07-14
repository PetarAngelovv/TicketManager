using GCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GCommon.GlobalValidation.Ticket;
namespace TicketManager.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [Range((double)MinPrice, (double)MaxPrice)]
        public decimal Price { get; set; }

        public bool IsSold { get; set; } = false;

        [Required]
        public int EventId { get; set; }

        public virtual Event Event { get; set; } = null!;

        public virtual ICollection<OrderTicket> OrderTickets { get; set; } = new HashSet<OrderTicket>();
    }
}
