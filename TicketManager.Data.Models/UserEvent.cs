﻿using Microsoft.AspNetCore.Identity;

namespace TicketManager.Data.Models
{
    public class UserEvent
    {
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        public int EventId { get; set; }
        public virtual Event Event { get; set; } = null!;
    }
}
