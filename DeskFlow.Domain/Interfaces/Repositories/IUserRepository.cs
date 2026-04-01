using DeskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces.Repositories
{
    public interface IUserRepository : ITenantScopedRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, Guid tenantId);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<IEnumerable<User>> GetAgentsByTenantAsync(Guid tenantId);
        Task<int> GetActiveUsersCountAsync(Guid tenantId);
        Task<bool> EmailExistsInTenantAsync(string email, Guid tenantId);
    }
}
