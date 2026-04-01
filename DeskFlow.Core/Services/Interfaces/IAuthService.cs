using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Auth;
using DeskFlow.Application.DTOs.Tenants;
using DeskFlow.Application.Services.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces
{
    public interface IAuthService :
        ICreatableService<RegisterTenantDto, Result<AuthResponseDto>>
    {
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<Result<bool>> LogoutAsync(Guid userId);
    }
}
