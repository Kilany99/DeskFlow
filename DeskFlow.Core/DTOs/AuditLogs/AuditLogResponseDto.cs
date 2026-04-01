using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.AuditLogs
{
    public class AuditLogResponseDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserFullName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
