using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Tickets;
using DeskFlow.Application.Services.Interfaces.Base;
using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces
{
    public interface ITicketService :
     ICreatableService<CreateTicketDto, Result<TicketResponseDto>>,
     IUpdatableService<UpdateTicketDto, Result<TicketResponseDto>>,
     ITenantScopedService<TicketResponseDto>
    {
        Task<Result<TicketResponseDto>> GetByReferenceNumberAsync(string referenceNumber);
        Task<Result<IEnumerable<TicketResponseDto>>> GetByAgentAsync(Guid agentId, Guid tenantId);
        Task<Result<IEnumerable<TicketResponseDto>>> GetByStatusAsync(TicketStatus status, Guid tenantId);
        Task<Result<TicketResponseDto>> AddReplyAsync(CreateReplyDto dto, Guid tenantId);
        Task<Result<TicketResponseDto>> AssignTicketAsync(Guid ticketId, Guid agentId, Guid tenantId);
        Task<Result<PagedResult<TicketResponseDto>>> GetPagedAsync(Guid tenantId, int page, int pageSize);
    }

}
