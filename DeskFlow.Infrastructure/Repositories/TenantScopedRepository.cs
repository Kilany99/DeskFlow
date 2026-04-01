using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace DeskFlow.Infrastructure.Repositories
{
    public abstract class TenantScopedRepository<T>
        : GenericRepository<T>, ITenantScopedRepository<T>
        where T : BaseEntity
    {
        protected abstract Expression<Func<T, Guid>> TenantIdSelector { get; }

        protected TenantScopedRepository(AppDbContext context) : base(context) { }

        public virtual async Task<IEnumerable<T>> GetAllByTenantAsync(Guid tenantId)
            => await _dbSet
                .Where(BuildTenantFilter(tenantId))
                .ToListAsync();

        public virtual async Task<T?> GetByIdAndTenantAsync(Guid id, Guid tenantId)
            => await _dbSet
                .Where(BuildTenantFilter(tenantId))
                .FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<bool> ExistsInTenantAsync(Guid id, Guid tenantId)
            => await _dbSet
                .Where(BuildTenantFilter(tenantId))
                .AnyAsync(e => e.Id == id);

        // Builds the tenantId == x filter dynamically
        private Expression<Func<T, bool>> BuildTenantFilter(Guid tenantId)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var property = Expression.Invoke(TenantIdSelector, param);
            var constant = Expression.Constant(tenantId);
            var equals = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(equals, param);
        }
    }
}
