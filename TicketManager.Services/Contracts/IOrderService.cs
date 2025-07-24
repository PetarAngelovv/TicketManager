namespace TicketManager.Services.Contracts
{
    public interface IOrderService
    {
        Task<bool> BuyTicketAsync(int eventId, string userId);
    }

}
