using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeSharingPlatform.Data.Configurations;
using System.Reflection;
using TicketManager.Data.Models;

namespace TicketManager.Data
{
    public class TicketManagerDbContext : IdentityDbContext
    {
        public TicketManagerDbContext(DbContextOptions<TicketManagerDbContext> options)
        : base(options) { }

        public virtual DbSet<Category> Categories => Set<Category>();
        public virtual DbSet<Event> Events => Set<Event>();
        public virtual DbSet<Ticket> Tickets => Set<Ticket>();
        public virtual DbSet<Order> Orders => Set<Order>();
        public virtual DbSet<OrderTicket> OrderTickets => Set<OrderTicket>();
        public virtual DbSet<UserEvent> UsersEvents  => Set<UserEvent>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }

}
