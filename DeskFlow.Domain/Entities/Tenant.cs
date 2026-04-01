using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public PlanType Plan { get; set; } = PlanType.Free;

        // Navigation
        public ICollection<User> Users { get; set; } = [];
        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
