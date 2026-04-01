using DeskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Interfaces.Repositories
{
    public interface ITicketReplyRepository : IRepository<TicketReply>
    {
        Task<IEnumerable<TicketReply>> GetByTicketIdAsync(Guid ticketId);
        Task<TicketReply?> GetLastReplyAsync(Guid ticketId);
    }
}
