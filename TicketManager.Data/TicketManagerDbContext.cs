using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManager.Data.Models;

namespace TicketManager.Data
{
    public class TicketManagerDbContext : IdentityDbContext
    {
        public TicketManagerDbContext(DbContextOptions<TicketManagerDbContext> options)
        : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderTicket> OrderTickets => Set<OrderTicket>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Category -> Event
            builder.Entity<Category>()
                .HasMany(c => c.Events)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event -> Ticket
            builder.Entity<Event>()
                .HasMany(e => e.Tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order -> OrderTicket
            builder.Entity<Order>()
                .HasMany(o => o.OrderTickets)
                .WithOne(ot => ot.Order)
                .HasForeignKey(ot => ot.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket -> OrderTicket
            builder.Entity<Ticket>()
                .HasMany(t => t.OrderTickets)
                .WithOne(ot => ot.Ticket)
                .HasForeignKey(ot => ot.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
               

            // Composite PK for OrderTicket
            builder.Entity<OrderTicket>()
                .HasKey(ot => new { ot.OrderId, ot.TicketId });

            builder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)"); 
        }
    }

}
