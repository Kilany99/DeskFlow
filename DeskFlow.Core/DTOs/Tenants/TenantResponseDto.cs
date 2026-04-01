using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Tenants
{
    public class TenantResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MemberCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
