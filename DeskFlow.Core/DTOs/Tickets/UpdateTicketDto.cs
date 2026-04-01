using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Tickets
{
    public class UpdateTicketDto
    {
        public TicketStatus? Status { get; set; }
        public TicketPriority? Priority { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
