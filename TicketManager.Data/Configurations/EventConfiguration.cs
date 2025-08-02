using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketManager.Data.Models;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            builder.Property(e => e.CreatedOn)
                .IsRequired();

            builder.Property(e => e.TicketPrice)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.TotalTickets)
                .IsRequired();

            builder.Property(e => e.CategoryId)
                .IsRequired();

            builder.HasMany(e => e.Tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(b => b.IsDeleted == false);

        } 
     
    }
}
