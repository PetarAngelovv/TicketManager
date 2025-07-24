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
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(b => b.IsDeleted == false);    
            builder.HasData(GenerateSeedEvents());
        }
    private List<Event> GenerateSeedEvents()
    {
        const string adminId = "fcf6a048-50ce-4fd6-a89b-2d95c88e607a";

        return new List<Event>()
        {
             new Event
             {
                 Id = 1,
                 Name = "Summer Music Festival",
                 Description = "An outdoor music festival with top artists and bands.",
                 CreatedOn = new DateTime(2025, 7, 20),
                 TicketPrice = 50.00m,
                 TotalTickets = 1000,
                 AuthorId = adminId,
                 CategoryId = 1
             },
             new Event
             {
                 Id = 2,
                 Name = "City Marathon",
                 Description = "Annual city marathon open for all participants.",
                 CreatedOn = new DateTime(2025, 9, 15),
                 TicketPrice = 30.00m,
                 TotalTickets = 500,
                 AuthorId = adminId,
                 CategoryId = 2
             },
             new Event
             {
                 Id = 3,
                 Name = "Tech Conference 2025",
                 Description = "Latest trends and talks in technology and innovation.",
                 CreatedOn = new DateTime(2025, 11, 10),
                 TicketPrice = 120.00m,
                 TotalTickets = 300,
                 AuthorId = adminId,
                 CategoryId = 4
            }
        };
    }
     
    }
}
