using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Interfaces.Repositories;
using DeskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Infrastructure.Repositories
{
    public class TicketReplyRepository : GenericRepository<TicketReply>, ITicketReplyRepository
    {
        public TicketReplyRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<TicketReply>> GetByTicketIdAsync(Guid ticketId)
            => await _dbSet
                .Where(r => r.TicketId == ticketId)
                .Include(r => r.Agent)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();

        public async Task<TicketReply?> GetLastReplyAsync(Guid ticketId)
            => await _dbSet
                .Where(r => r.TicketId == ticketId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();
    }
}
