using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public Guid TenantId { get; set; }
        public Guid? AssignedToUserId { get; set; }

        // Customer info 
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public string ReferenceNumber { get; set; } = string.Empty; // TKT-00042
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        // Navigation
        public Tenant Tenant { get; set; } = null!;
        public User? AssignedTo { get; set; }
        public ICollection<TicketReply> Replies { get; set; } = [];

    }
}
