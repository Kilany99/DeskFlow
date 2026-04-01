using DeskFlow.Domain.Interfaces;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public ITicketRepository Tickets { get; }
        public IUserRepository Users { get; }
        public ITicketReplyRepository TicketReplies { get; }
        public ITenantRepository Tenants { get; }
        public IAuditLogRepository AuditLogs { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Tickets = new TicketRepository(context);
            Users = new UserRepository(context);
            TicketReplies = new TicketReplyRepository(context);
            Tenants = new TenantRepository(context);
            AuditLogs = new AuditLogRepository(context);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
