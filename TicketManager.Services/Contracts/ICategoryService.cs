using TicketManager.Web.ViewModels.Event;

namespace TicketManager.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<AddCategoryDropDownModel>> GetCategoriesDropDownAsync();
    }
}
