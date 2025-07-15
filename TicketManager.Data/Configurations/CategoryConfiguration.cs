using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static GCommon.GlobalValidation.Category;

namespace TicketManager.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            // Връзка 1 към много: Category -> Events
            builder.HasMany(c => c.Events)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(GenerateSeedCategories());
        }

        private List<Category> GenerateSeedCategories()
        {
            return new List<Category>()
            {
                new Category { Id = 1, Name = "Concert" },
                new Category { Id = 2, Name = "Sports" },
                new Category { Id = 3, Name = "Theatre" },
                new Category { Id = 4, Name = "Conference" },
                new Category { Id = 5, Name = "Festival" }
            };
        }
    }
}
