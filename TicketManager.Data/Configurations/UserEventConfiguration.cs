using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TicketManager.Data.Models;

namespace TicketManager.Data.Configurations
{
    public class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.EventId });

            builder.HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ur => ur.Event)
                .WithMany(r => r.UsersEvents)
                .HasForeignKey(ur => ur.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(ud => ud.Event.IsDeleted == false);
        }
    }
}
