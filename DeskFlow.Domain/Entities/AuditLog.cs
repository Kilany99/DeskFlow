using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public Guid TenantId { get; set; }
        public Guid? UserId { get; set; }         

        public string Action { get; set; } = string.Empty;   
        public string? Details { get; set; }                  
        public string? IpAddress { get; set; }

        // Navigation
        public Tenant Tenant { get; set; } = null!;
        public User? User { get; set; }
    }
}
