namespace TicketManager.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsManager { get; set; }
    }
}
