using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RecipeSharingPlatform.Data.Configurations
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
           builder.HasData(this.GenerateSeedUser());
        }
        public IdentityUser GenerateSeedUser()
        {
            var defaultUser = new IdentityUser
            {
                Id = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                UserName = "admin@TManager.com",
                NormalizedUserName = "ADMIN@TMANAGER.COM",
                Email = "admin@TManager.com",
                NormalizedEmail = "ADMIN@TMANAGER.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                    new IdentityUser { UserName = "admin@TManager.com" },
                    "Admin123!")
            };
            return defaultUser;
           
        }
    }
}
