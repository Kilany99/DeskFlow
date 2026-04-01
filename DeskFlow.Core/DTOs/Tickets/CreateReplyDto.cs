using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Tickets
{
    public class CreateReplyDto
    {
        public Guid TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsFromCustomer { get; set; }
    }

}
