using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Infrastructure.Repositories
{
    public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(AppDbContext context) : base(context) { }

        public async Task<Tenant?> GetBySubdomainAsync(string subdomain)
            => await _dbSet.FirstOrDefaultAsync(t => t.Subdomain == subdomain);

        public async Task<bool> SubdomainExistsAsync(string subdomain)
            => await _dbSet.AnyAsync(t => t.Subdomain == subdomain);

        public async Task<IEnumerable<Tenant>> GetActiveTenantsAsync()
            => await _dbSet.Where(t => t.IsActive).ToListAsync();
    }
}
