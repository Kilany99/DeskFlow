using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.AuditLogs;
using DeskFlow.Application.Services.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces
{
    public interface IAuditLogService :
     ITenantScopedService<AuditLogResponseDto>
    {
        Task LogAsync(Guid tenantId, Guid? userId, string action, string? details = null);
        Task<Result<IEnumerable<AuditLogResponseDto>>> GetByUserAsync(Guid userId, Guid tenantId);
        Task<Result<IEnumerable<AuditLogResponseDto>>> GetAllLogsAsync(); // Super Admin only
    }
}
