using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Entities
{
    public class TicketReply : BaseEntity
    {
        public Guid TicketId { get; set; }
        public Guid? AgentId { get; set; }       // null = customer reply

        public string Message { get; set; } = string.Empty;
        public bool IsFromCustomer { get; set; }

        // Navigation
        public Ticket Ticket { get; set; } = null!;
        public User? Agent { get; set; }
    }
}
