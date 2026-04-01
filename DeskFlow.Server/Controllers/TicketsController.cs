using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Tickets;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Enums;
using DeskFlow.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.Server.Controllers
{
    [Authorize]
    public class TicketsController : TenantScopedController
    {
        private readonly ITicketService _ticketService;
        private readonly IAuditLogService _auditLogService;

        public TicketsController(
            ITicketService ticketService,
            IAuditLogService auditLogService)
        {
            _ticketService = ticketService;
            _auditLogService = auditLogService;
        }

        /// <summary>Get all tickets for the current tenant (paged)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
            => HandleResult(await _ticketService.GetPagedAsync(
                ResolveTenantId(), page, pageSize));

        /// <summary>Get ticket by id scoped to tenant</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ticketService.GetByIdAndTenantAsync(id, ResolveTenantId());
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>Get ticket by reference number (public — no auth needed)</summary>
        [HttpGet("track/{referenceNumber}")]
        [AllowAnonymous]
        public async Task<IActionResult> Track(string referenceNumber)
            => HandleResult(await _ticketService.GetByReferenceNumberAsync(referenceNumber));

        /// <summary>Get tickets by status</summary>
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(TicketStatus status)
            => HandleResult(await _ticketService.GetByStatusAsync(status, ResolveTenantId()));

        /// <summary>Get tickets assigned to current agent</summary>
        [HttpGet("my")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> GetMyTickets()
            => HandleResult(await _ticketService.GetByAgentAsync(
                CurrentUserId, ResolveTenantId()));

        /// <summary>Submit a new ticket (public — no auth needed)</summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
          [FromBody] CreateTicketDto dto,
          [FromHeader(Name = "X-Tenant-Id")] Guid tenantId)
        {
            dto.TenantId = tenantId; // tenant comes from header on public endpoint
            var result = await _ticketService.CreateAsync(dto);

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    tenantId,
                    null,
                    AuditActions.TicketCreated,
                    $"Ticket {result.Data!.ReferenceNumber} created by {dto.CustomerEmail}");

            return HandleResult(result);
        }

        /// <summary>Update ticket status, priority, or assignment</summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Agent,TenantAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto dto)
        {
            var result = await _ticketService.UpdateAsync(id, dto);

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    ResolveTenantId(),
                    CurrentUserId,
                    AuditActions.TicketUpdated,
                    $"Ticket {id} updated");

            return HandleResult(result);
        }

        /// <summary>Assign ticket to an agent</summary>
        [HttpPatch("{id:guid}/assign/{agentId:guid}")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> Assign(Guid id, Guid agentId)
        {
            var result = await _ticketService.AssignTicketAsync(
                id, agentId, ResolveTenantId());

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    ResolveTenantId(),
                    CurrentUserId,
                    AuditActions.TicketAssigned,
                    $"Ticket {id} assigned to agent {agentId}");

            return HandleResult(result);
        }

        /// <summary>Add a reply to a ticket</summary>
        [HttpPost("{id:guid}/reply")]
        [AllowAnonymous]
        public async Task<IActionResult> Reply(Guid id, [FromBody] CreateReplyDto dto)
        {
            dto.TicketId = id;
            var result = await _ticketService.AddReplyAsync(dto, ResolveTenantId());

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    ResolveTenantId(),
                    dto.IsFromCustomer ? null : CurrentUserId,
                    AuditActions.TicketReplied,
                    $"Reply added to ticket {id}");

            return HandleResult(result);
        }
    }
}
