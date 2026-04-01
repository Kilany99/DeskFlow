using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid TenantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        // Navigation
        public Tenant Tenant { get; set; } = null!;
        public ICollection<Ticket> AssignedTickets { get; set; } = [];
        public ICollection<TicketReply> Replies { get; set; } = [];
    }
}
