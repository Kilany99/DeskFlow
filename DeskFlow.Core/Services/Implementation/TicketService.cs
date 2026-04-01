using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Tickets;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using DeskFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _uow;

        public TicketService(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<TicketResponseDto>> CreateAsync(CreateTicketDto dto)
        {
            var referenceNumber = await _uow.Tickets.GenerateReferenceNumberAsync();

            var ticket = new Ticket
            {
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                Subject = dto.Subject,
                Description = dto.Description,
                Priority = dto.Priority,
                ReferenceNumber = referenceNumber,
                Status = TicketStatus.Open
            };

            await _uow.Tickets.AddAsync(ticket);
            await _uow.SaveChangesAsync();

            return Result<TicketResponseDto>.Success(MapToDto(ticket), 201);
        }

        public async Task<Result<TicketResponseDto>> UpdateAsync(Guid id, UpdateTicketDto dto)
        {
            var ticket = await _uow.Tickets.GetByIdAsync(id);
            if (ticket is null)
                return Result<TicketResponseDto>.NotFound("Ticket not found");

            if (dto.Status.HasValue) ticket.Status = dto.Status.Value;
            if (dto.Priority.HasValue) ticket.Priority = dto.Priority.Value;
            if (dto.AssignedToUserId.HasValue) ticket.AssignedToUserId = dto.AssignedToUserId;

            await _uow.Tickets.UpdateAsync(ticket);
            await _uow.SaveChangesAsync();

            return Result<TicketResponseDto>.Success(MapToDto(ticket));
        }

        public async Task<Result<TicketResponseDto>> AddReplyAsync(CreateReplyDto dto, Guid tenantId)
        {
            var ticket = await _uow.Tickets.GetByIdAndTenantAsync(dto.TicketId, tenantId);
            if (ticket is null)
                return Result<TicketResponseDto>.NotFound("Ticket not found");

            var reply = new TicketReply
            {
                TicketId = dto.TicketId,
                Message = dto.Message,
                IsFromCustomer = dto.IsFromCustomer
            };

            await _uow.TicketReplies.AddAsync(reply);

            if (!dto.IsFromCustomer && ticket.Status == TicketStatus.Open)
                ticket.Status = TicketStatus.InProgress;

            await _uow.Tickets.UpdateAsync(ticket);
            await _uow.SaveChangesAsync();

            return Result<TicketResponseDto>.Success(MapToDto(ticket));
        }

        public async Task<Result<TicketResponseDto>> AssignTicketAsync(
            Guid ticketId, Guid agentId, Guid tenantId)
        {
            var ticket = await _uow.Tickets.GetByIdAndTenantAsync(ticketId, tenantId);
            if (ticket is null)
                return Result<TicketResponseDto>.NotFound("Ticket not found");

            var agent = await _uow.Users.GetByIdAndTenantAsync(agentId, tenantId);
            if (agent is null || !agent.IsActive)
                return Result<TicketResponseDto>.Failure("Agent not found or inactive");

            ticket.AssignedToUserId = agentId;
            await _uow.Tickets.UpdateAsync(ticket);
            await _uow.SaveChangesAsync();

            return Result<TicketResponseDto>.Success(MapToDto(ticket));
        }

        public async Task<Result<TicketResponseDto>> GetByReferenceNumberAsync(string referenceNumber)
        {
            var ticket = await _uow.Tickets.GetByReferenceNumberAsync(referenceNumber);
            return ticket is null
                ? Result<TicketResponseDto>.NotFound()
                : Result<TicketResponseDto>.Success(MapToDto(ticket));
        }

        public async Task<Result<IEnumerable<TicketResponseDto>>> GetByAgentAsync(
            Guid agentId, Guid tenantId)
        {
            var tickets = await _uow.Tickets.GetByAssignedAgentAsync(agentId, tenantId);
            return Result<IEnumerable<TicketResponseDto>>.Success(tickets.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<TicketResponseDto>>> GetByStatusAsync(
            TicketStatus status, Guid tenantId)
        {
            var tickets = await _uow.Tickets.GetByStatusAsync(status, tenantId);
            return Result<IEnumerable<TicketResponseDto>>.Success(tickets.Select(MapToDto));
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllByTenantAsync(Guid tenantId)
        {
            var tickets = await _uow.Tickets.GetAllByTenantAsync(tenantId);
            return tickets.Select(MapToDto);
        }

        public async Task<TicketResponseDto?> GetByIdAndTenantAsync(Guid id, Guid tenantId)
        {
            var ticket = await _uow.Tickets.GetByIdAndTenantAsync(id, tenantId);
            return ticket is null ? null : MapToDto(ticket);
        }

        public async Task<Result<PagedResult<TicketResponseDto>>> GetPagedAsync(
            Guid tenantId, int page, int pageSize)
        {
            var all = await _uow.Tickets.GetAllByTenantAsync(tenantId);
            var paged = all
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return Result<PagedResult<TicketResponseDto>>.Success(new PagedResult<TicketResponseDto>
            {
                Items = paged.Select(MapToDto),
                TotalCount = all.Count(),
                Page = page,
                PageSize = pageSize
            });
        }

        private static TicketResponseDto MapToDto(Ticket t) => new()
        {
            Id = t.Id,
            ReferenceNumber = t.ReferenceNumber,
            CustomerName = t.CustomerName,
            CustomerEmail = t.CustomerEmail,
            Subject = t.Subject,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            AssignedToName = t.AssignedTo?.FullName,
            CreatedAt = t.CreatedAt,
            Replies = t.Replies?.Select(r => new TicketReplyResponseDto
            {
                Id = r.Id,
                Message = r.Message,
                IsFromCustomer = r.IsFromCustomer,
                AgentName = r.Agent?.FullName,
                CreatedAt = r.CreatedAt
            }).ToList() ?? []
        };
    }

}
