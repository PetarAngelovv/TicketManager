namespace TicketManager.Services.Contracts
{
    public interface IRoleSeederService
    {
        Task SeedRolesAsync();
        Task SeedAdminAsync();
    }
}
