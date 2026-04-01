using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Tickets
{
    public class TicketResponseDto
    {
        public Guid Id { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string? AssignedToName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketReplyResponseDto> Replies { get; set; } = [];
    }

}
