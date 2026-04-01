using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.Server.Controllers
{
    [Authorize]
    public class AuditLogsController : TenantScopedController
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
            => _auditLogService = auditLogService;

        /// <summary>Get all logs for the current tenant (TenantAdmin)</summary>
        [HttpGet]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> GetByTenant()
        {
            var logs = await _auditLogService.GetAllByTenantAsync(ResolveTenantId());
            return Ok(logs);
        }

        /// <summary>Get logs for a specific user in the tenant</summary>
        [HttpGet("user/{userId:guid}")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> GetByUser(Guid userId)
            => HandleResult(await _auditLogService.GetByUserAsync(userId, ResolveTenantId()));

        /// <summary>Get all logs across all tenants (SuperAdmin only)</summary>
        [HttpGet("all")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
            => HandleResult(await _auditLogService.GetAllLogsAsync());
    }
}
