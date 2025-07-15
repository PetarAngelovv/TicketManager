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
                .HasPrecision(18, 2) // например, за decimal

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
                      AuthorId = "cb79d2b9-3231-408b-83d3-30c6879aa313",
                    CategoryId = 1 // напр. "Concert"
                },
                new Event
                {
                    Id = 2,
                    Name = "City Marathon",
                    Description = "Annual city marathon open for all participants.",
                    CreatedOn = new DateTime(2025, 9, 15),
                    TicketPrice = 30.00m,
                    TotalTickets = 500,
                       AuthorId = "929dbb49-522c-4f84-a486-5d11e35fd7fc",
                    CategoryId = 2 // напр. "Sports"
                },
                new Event
                {
                    Id = 3,
                    Name = "Tech Conference 2025",
                    Description = "Latest trends and talks in technology and innovation.",
                    CreatedOn = new DateTime(2025, 11, 10),
                    TicketPrice = 120.00m,
                    TotalTickets = 300,
                       AuthorId = "1d48e3d2-08e1-4162-8c51-65f0723a017f",
                    CategoryId = 4 // напр. "Conference"
                }
            };
        }
    }
}
