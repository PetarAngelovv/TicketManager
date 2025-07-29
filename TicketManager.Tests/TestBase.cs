using Microsoft.EntityFrameworkCore;
using TicketManager.Data;

namespace TicketManager.Tests
{
    public abstract class TestBase
    {
        protected TicketManagerDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TicketManagerDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new TicketManagerDbContext(options);
        }
    }
}