using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces.Repositories
{
    public interface ITicketRepository : ITenantScopedRepository<Ticket>
    {
        Task<Ticket?> GetByReferenceNumberAsync(string referenceNumber);
        Task<IEnumerable<Ticket>> GetByAssignedAgentAsync(Guid agentId, Guid tenantId);
        Task<IEnumerable<Ticket>> GetByStatusAsync(TicketStatus status, Guid tenantId);
        Task<int> GetOpenTicketsCountAsync(Guid tenantId);
        Task<string> GenerateReferenceNumberAsync();
    }

}
