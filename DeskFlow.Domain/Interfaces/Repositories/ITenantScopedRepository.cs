using DeskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces.Repositories
{
    public interface ITenantScopedRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllByTenantAsync(Guid tenantId);
        Task<T?> GetByIdAndTenantAsync(Guid id, Guid tenantId);
        Task<bool> ExistsInTenantAsync(Guid id, Guid tenantId);
    }
}
