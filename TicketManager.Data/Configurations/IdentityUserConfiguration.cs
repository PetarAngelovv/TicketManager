using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketManager.Data.Configurations
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.HasData(this.GenerateSeedUsers());
        }

        private List<IdentityUser> GenerateSeedUsers()
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();

            var admin = new IdentityUser
            {
                Id = "fcf6a048-50ce-4fd6-a89b-2d95c88e607a",
                UserName = "admin@TManager.com",
                NormalizedUserName = "ADMIN@TMANAGER.COM",
                Email = "admin@TManager.com",
                NormalizedEmail = "ADMIN@TMANAGER.COM",
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null, "Admin123!")
            };

            var manager = new IdentityUser
            {
                Id = "e14720aa-73e1-4f8f-8f1f-6736cfe1a00b",
                UserName = "manager@TManager.com",
                NormalizedUserName = "MANAGER@TMANAGER.COM",
                Email = "manager@TManager.com",
                NormalizedEmail = "MANAGER@TMANAGER.COM",
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null, "Manager123!")
            };

            var user = new IdentityUser
            {
                Id = "b3102d7f-82cb-470c-88d3-a299a1e8c1b9",
                UserName = "user@TManager.com",
                NormalizedUserName = "USER@TMANAGER.COM",
                Email = "user@TManager.com",
                NormalizedEmail = "USER@TMANAGER.COM",
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null, "User123!")
            };

            return new List<IdentityUser> { admin, manager, user };
        }
    }
}
