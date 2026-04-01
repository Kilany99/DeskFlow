using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Tenants;
using DeskFlow.Application.Services.Interfaces.Base;
using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces
{
    public interface ITenantService :
      IReadableService<TenantResponseDto>,
      IActivatableService
    {
        Task<Result<IEnumerable<TenantResponseDto>>> GetAllTenantsAsync();
        Task<Result<TenantResponseDto>> GetBySubdomainAsync(string subdomain);
        Task<Result<TenantResponseDto>> ChangePlanAsync(Guid tenantId, PlanType plan);
        Task<Result<bool>> CanInviteMoreUsersAsync(Guid tenantId);
    }
}
