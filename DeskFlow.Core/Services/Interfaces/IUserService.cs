using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Users;
using DeskFlow.Application.Services.Interfaces.Base;
using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces
{
    public interface IUserService :
      IReadableService<UserResponseDto>,
      IUpdatableService<UpdateUserDto, Result<UserResponseDto>>,
      IActivatableService,
      ITenantScopedService<UserResponseDto>
    {
        Task<Result<UserResponseDto>> InviteUserAsync(InviteUserDto dto, Guid tenantId);
        Task<Result<IEnumerable<UserResponseDto>>> GetAgentsAsync(Guid tenantId);
        Task<Result<bool>> ChangeRoleAsync(Guid userId, UserRole role, Guid tenantId);
    }
}
