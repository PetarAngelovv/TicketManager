using Microsoft.EntityFrameworkCore;
using TicketManager.Data;
using TicketManager.Services.Contracts;
using TicketManager.Web.ViewModels.Event;

namespace TicketManager.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly TicketManagerDbContext _dbContext;

        public CategoryService(TicketManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AddCategoryDropDownModel>> GetCategoriesDropDownAsync()
        {
            var categories = await _dbContext.Categories
                .AsNoTracking()
                .Select(c => new AddCategoryDropDownModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }
    }
}
