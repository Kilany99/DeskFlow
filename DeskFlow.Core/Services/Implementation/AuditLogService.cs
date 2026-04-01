using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.AuditLogs;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Implementation
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _uow;

        public AuditLogService(IUnitOfWork uow) => _uow = uow;

        public async Task LogAsync(Guid tenantId, Guid? userId, string action, string? details = null)
        {
            await _uow.AuditLogs.LogAsync(tenantId, userId, action, details);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLogResponseDto>> GetAllByTenantAsync(Guid tenantId)
        {
            var logs = await _uow.AuditLogs.GetAllByTenantAsync(tenantId);
            return logs.Select(MapToDto);
        }

        public async Task<AuditLogResponseDto?> GetByIdAndTenantAsync(Guid id, Guid tenantId)
        {
            var log = await _uow.AuditLogs.GetByIdAndTenantAsync(id, tenantId);
            return log is null ? null : MapToDto(log);
        }

        public async Task<Result<IEnumerable<AuditLogResponseDto>>> GetByUserAsync(
            Guid userId, Guid tenantId)
        {
            var logs = await _uow.AuditLogs.GetByUserAsync(userId, tenantId);
            return Result<IEnumerable<AuditLogResponseDto>>.Success(logs.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<AuditLogResponseDto>>> GetAllLogsAsync()
        {
            var logs = await _uow.AuditLogs.GetAllAsync();
            return Result<IEnumerable<AuditLogResponseDto>>.Success(logs.Select(MapToDto));
        }

        private static AuditLogResponseDto MapToDto(AuditLog a) => new()
        {
            Id = a.Id,
            TenantId = a.TenantId,
            UserId = a.UserId,
            UserFullName = a.User?.FullName,
            Action = a.Action,
            Details = a.Details,
            IpAddress = a.IpAddress,
            CreatedAt = a.CreatedAt
        };
    }
}
