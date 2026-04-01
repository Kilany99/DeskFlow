using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DeskFlow.Infrastructure.Repositories
{
    public class TicketRepository : TenantScopedRepository<Ticket>, ITicketRepository
    {
        protected override Expression<Func<Ticket, Guid>> TenantIdSelector
            => t => t.TenantId;

        public TicketRepository(AppDbContext context) : base(context) { }

        public async Task<Ticket?> GetByReferenceNumberAsync(string referenceNumber)
            => await _dbSet
                .Include(t => t.Replies)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.ReferenceNumber == referenceNumber);

        public async Task<IEnumerable<Ticket>> GetByAssignedAgentAsync(Guid agentId, Guid tenantId)
            => await _dbSet
                .Where(t => t.TenantId == tenantId && t.AssignedToUserId == agentId)
                .Include(t => t.Replies)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByStatusAsync(TicketStatus status, Guid tenantId)
            => await _dbSet
                .Where(t => t.TenantId == tenantId && t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<int> GetOpenTicketsCountAsync(Guid tenantId)
            => await _dbSet
                .CountAsync(t => t.TenantId == tenantId && t.Status == TicketStatus.Open);

        public async Task<string> GenerateReferenceNumberAsync()
        {
            var count = await _dbSet.CountAsync();
            return $"TKT-{(count + 1):D5}";
        }
    }
}
