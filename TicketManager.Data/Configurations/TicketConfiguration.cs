using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketManager.Data.Models;

namespace TicketManager.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(18, 2) // decimal(18,2)
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.IsSold)
                .HasDefaultValue(false);

            builder.Property(t => t.EventId)
                .IsRequired();

            builder.HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.OrderTickets)
                .WithOne(ot => ot.Ticket)
                .HasForeignKey(ot => ot.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
