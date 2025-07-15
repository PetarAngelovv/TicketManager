using Microsoft.AspNetCore.Identity;
using TicketManager.Services.Contracts;

public class RoleSeederService : IRoleSeederService
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<IdentityUser> userManager;

    public RoleSeederService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    public async Task SeedRolesAsync()
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Manager"))
        {
            await roleManager.CreateAsync(new IdentityRole("Manager"));
        }
    }
    public async Task SeedAdminAsync()
    {
        var adminEmail = "admin@TManager.com";

        // Проверка дали съществува потребител с този email
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            // Създаване на потребителя с парола
            await userManager.CreateAsync(adminUser, "Admin123!");
        }

        // Добавяне на потребителя към ролята Admin, ако още не е
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
