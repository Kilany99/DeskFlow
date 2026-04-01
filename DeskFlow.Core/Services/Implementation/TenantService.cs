using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Tenants;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using DeskFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Implementation
{
    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _uow;

        public TenantService(IUnitOfWork uow) => _uow = uow;

        public async Task<TenantResponseDto?> GetByIdAsync(Guid id)
        {
            var tenant = await _uow.Tenants.GetByIdAsync(id);
            return tenant is null ? null : MapToDto(tenant);
        }

        public async Task<IEnumerable<TenantResponseDto>> GetAllAsync()
        {
            var tenants = await _uow.Tenants.GetAllAsync();
            return tenants.Select(MapToDto);
        }

        public async Task<Result<IEnumerable<TenantResponseDto>>> GetAllTenantsAsync()
        {
            var tenants = await _uow.Tenants.GetAllAsync();
            return Result<IEnumerable<TenantResponseDto>>.Success(tenants.Select(MapToDto));
        }

        public async Task<Result<TenantResponseDto>> GetBySubdomainAsync(string subdomain)
        {
            var tenant = await _uow.Tenants.GetBySubdomainAsync(subdomain);
            return tenant is null
                ? Result<TenantResponseDto>.NotFound("Tenant not found")
                : Result<TenantResponseDto>.Success(MapToDto(tenant));
        }

        public async Task<Result<TenantResponseDto>> ChangePlanAsync(Guid tenantId, PlanType plan)
        {
            var tenant = await _uow.Tenants.GetByIdAsync(tenantId);
            if (tenant is null)
                return Result<TenantResponseDto>.NotFound("Tenant not found");

            tenant.Plan = plan;
            await _uow.Tenants.UpdateAsync(tenant);
            await _uow.SaveChangesAsync();

            return Result<TenantResponseDto>.Success(MapToDto(tenant));
        }

        public async Task<Result<bool>> CanInviteMoreUsersAsync(Guid tenantId)
        {
            var tenant = await _uow.Tenants.GetByIdAsync(tenantId);
            if (tenant is null)
                return Result<bool>.NotFound("Tenant not found");

            var currentCount = await _uow.Users.GetActiveUsersCountAsync(tenantId);

            var limit = tenant.Plan switch
            {
                PlanType.Free => 5,
                PlanType.Pro => 50,
                PlanType.Enterprise => int.MaxValue,
                _ => 5
            };

            return currentCount < limit
                ? Result<bool>.Success(true)
                : Result<bool>.Failure($"Plan limit of {limit} members reached. Please upgrade.");
        }

        public async Task ActivateAsync(Guid id)
        {
            var tenant = await _uow.Tenants.GetByIdAsync(id);
            if (tenant is null) return;

            tenant.IsActive = true;
            await _uow.Tenants.UpdateAsync(tenant);
            await _uow.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Guid id)
        {
            var tenant = await _uow.Tenants.GetByIdAsync(id);
            if (tenant is null) return;

            tenant.IsActive = false;
            await _uow.Tenants.UpdateAsync(tenant);
            await _uow.SaveChangesAsync();
        }

        private static TenantResponseDto MapToDto(Tenant t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            Subdomain = t.Subdomain,
            Plan = t.Plan.ToString(),
            IsActive = t.IsActive,
            MemberCount = t.Users?.Count ?? 0,
            CreatedAt = t.CreatedAt
        };
    }
}
