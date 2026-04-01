using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DeskFlow.Infrastructure.Repositories
{
    public class AuditLogRepository : TenantScopedRepository<AuditLog>, IAuditLogRepository
    {
        protected override Expression<Func<AuditLog, Guid>> TenantIdSelector
            => a => a.TenantId;

        public AuditLogRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, Guid tenantId)
            => await _dbSet
                .Where(a => a.TenantId == tenantId && a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task LogAsync(Guid tenantId, Guid? userId, string action, string? details = null)
        {
            await AddAsync(new AuditLog
            {
                TenantId = tenantId,
                UserId = userId,
                Action = action,
                Details = details
            });
        }
    }
}
