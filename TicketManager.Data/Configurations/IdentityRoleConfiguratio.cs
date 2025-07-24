using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketManager.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "9d98f75a-68cb-4b56-a6b8-b4b2a7061a9f", 
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "1a2b3c4d-73e1-4f8f-8f1f-6736cfe1a00b",  
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Id = "2b3c4d5e-82cb-470c-88d3-a299a1e8c1b9",  
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
        }
    }
}
