using DeskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces.Repositories
{
    public interface IAuditLogRepository : ITenantScopedRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, Guid tenantId);
        Task LogAsync(Guid tenantId, Guid? userId, string action, string? details = null);
    }
}
