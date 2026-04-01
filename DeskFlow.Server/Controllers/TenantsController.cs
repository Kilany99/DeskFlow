using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Tenants;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.Server.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TenantsController : BaseApiController
    {
        private readonly ITenantService _tenantService;
        private readonly IAuditLogService _auditLogService;

        public TenantsController(
            ITenantService tenantService,
            IAuditLogService auditLogService)
        {
            _tenantService = tenantService;
            _auditLogService = auditLogService;
        }

        /// <summary>Get all tenants on the platform</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => HandleResult(await _tenantService.GetAllTenantsAsync());

        /// <summary>Get a tenant by id</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tenant = await _tenantService.GetByIdAsync(id);
            return tenant is null ? NotFound() : Ok(tenant);
        }

        /// <summary>Change a tenant's subscription plan</summary>
        [HttpPatch("{id:guid}/plan")]
        public async Task<IActionResult> ChangePlan(Guid id, [FromBody] ChangePlanDto dto)
        {
            var result = await _tenantService.ChangePlanAsync(id, dto.Plan);

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    id,
                    CurrentUserId,
                    AuditActions.PlanChanged,
                    $"Plan changed to {dto.Plan}");

            return HandleResult(result);
        }

        /// <summary>Suspend a tenant — all their users can no longer log in</summary>
        [HttpPatch("{id:guid}/suspend")]
        public async Task<IActionResult> Suspend(Guid id)
        {
            await _tenantService.DeactivateAsync(id);

            await _auditLogService.LogAsync(
                id,
                CurrentUserId,
                AuditActions.TenantSuspended,
                $"Tenant {id} suspended");

            return NoContent();
        }

        /// <summary>Unsuspend a tenant</summary>
        [HttpPatch("{id:guid}/unsuspend")]
        public async Task<IActionResult> Unsuspend(Guid id)
        {
            await _tenantService.ActivateAsync(id);
            return NoContent();
        }
    }
}
