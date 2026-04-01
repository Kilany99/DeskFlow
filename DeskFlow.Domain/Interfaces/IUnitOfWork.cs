using DeskFlow.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for coordinating changes across multiple repositories as a single unit of work, supporting
    /// transactional operations and asynchronous persistence.
    /// </summary>
    /// <remarks>The unit of work pattern ensures that a set of related changes to the data store are
    /// committed or rolled back as a single transaction. This interface provides access to repositories for managing
    /// tickets, users, ticket replies, tenants, and audit logs, and exposes methods for transaction management and
    /// saving changes asynchronously. Implementations should ensure thread safety and proper disposal of resources.
    /// Typical usage involves beginning a transaction, performing operations through the repositories, and then
    /// committing or rolling back the transaction as needed.</remarks>
    public interface IUnitOfWork : IDisposable
    {
        ITicketRepository Tickets { get; }
        IUserRepository Users { get; }
        ITicketReplyRepository TicketReplies { get; }
        ITenantRepository Tenants { get; }
        IAuditLogRepository AuditLogs { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
