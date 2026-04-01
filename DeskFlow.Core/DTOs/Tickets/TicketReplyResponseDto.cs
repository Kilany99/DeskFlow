using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Tickets
{
    public class TicketReplyResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsFromCustomer { get; set; }
        public string? AgentName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
