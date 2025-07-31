using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketManager.Data;
using TicketManager.Services;
using TicketManager.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<TicketManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<TicketManagerDbContext>();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
    })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
        options.Scope.Add("user:email");
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRoleSeederService, RoleSeederService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<TicketManagerDbContext>();
    context.Database.EnsureCreated();

    var roleSeeder = scope.ServiceProvider.GetRequiredService<IRoleSeederService>();
    await roleSeeder.SeedAsync();
    
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();