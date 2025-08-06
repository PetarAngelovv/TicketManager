using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketManager.Data.Models;

namespace TicketManager.Data.Configurations
{
    public class OrderTicketConfiguration : IEntityTypeConfiguration<OrderTicket>
    {
        public void Configure(EntityTypeBuilder<OrderTicket> builder)
        {
            builder.HasKey(ot => new { ot.OrderId, ot.TicketId });

            builder.HasOne(ot => ot.Order)
                .WithMany(o => o.OrderTickets)
                .HasForeignKey(ot => ot.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ot => ot.Ticket)
                .WithMany(t => t.OrderTickets)
                .HasForeignKey(ot => ot.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
