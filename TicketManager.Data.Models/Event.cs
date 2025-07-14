using GCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Data.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(LocationMaxLength, MinimumLength = LocationMinLength)]
        public string Location { get; set; } = null!;

        [Required]
        public virtual DateTime StartTime { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}