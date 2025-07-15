using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManager.Services.Contracts
{
    public interface IRoleSeederService
    {
        Task SeedRolesAsync();
        Task SeedAdminAsync();
    }
}
