using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces.Base
{
    public interface ITenantScopedService<TResponse>
    {
        Task<IEnumerable<TResponse>> GetAllByTenantAsync(Guid tenantId);
        Task<TResponse?> GetByIdAndTenantAsync(Guid id, Guid tenantId);
    }
}
