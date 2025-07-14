using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManager.Web.ViewModels.Event
{
    public class EventDeleteInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public  string CreatedOn { get; set; } = null!;

        public decimal TicketPrice { get; set; }

        public string? Author { get; set; }
        public string AuthorId { get; set; } = null!;
    }
}
