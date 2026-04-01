using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.Infrastructure.Repositories
{
    public class UserRepository : TenantScopedRepository<User>, IUserRepository
    {
        protected override Expression<Func<User, Guid>> TenantIdSelector
            => u => u.TenantId;

        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, Guid tenantId)
            => await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && u.TenantId == tenantId);

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
            => await _dbSet
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken
                                       && u.RefreshTokenExpiry > DateTime.UtcNow);

        public async Task<IEnumerable<User>> GetAgentsByTenantAsync(Guid tenantId)
            => await _dbSet
                .Where(u => u.TenantId == tenantId && u.Role == UserRole.Agent && u.IsActive)
                .ToListAsync();

        public async Task<int> GetActiveUsersCountAsync(Guid tenantId)
            => await _dbSet
                .CountAsync(u => u.TenantId == tenantId && u.IsActive);

        public async Task<bool> EmailExistsInTenantAsync(string email, Guid tenantId)
            => await _dbSet
                .AnyAsync(u => u.Email == email && u.TenantId == tenantId);
    }
}
