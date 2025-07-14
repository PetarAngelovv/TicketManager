using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManager.Data
{
    public class ApplicationDbInitializer
    {
        private const string AdminEmail = "admin@nba.com";
        private const string AdminPassword = "Admin123!";

        private static readonly string[] Roles = new[] { "Admin", "User" };

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Създаване на роли
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Създаване на админ потребител
            if (await userManager.FindByEmailAsync(AdminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception("Неуспешно създаване на админ: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
