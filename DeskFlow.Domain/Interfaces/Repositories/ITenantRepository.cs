using DeskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces.Repositories
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        Task<Tenant?> GetBySubdomainAsync(string subdomain);
        Task<bool> SubdomainExistsAsync(string subdomain);
        Task<IEnumerable<Tenant>> GetActiveTenantsAsync();
    }
}
