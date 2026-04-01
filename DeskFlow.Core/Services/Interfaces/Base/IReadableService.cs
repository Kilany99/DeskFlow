using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces.Base
{
    public interface IReadableService<TResponse>
    {
        Task<TResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<TResponse>> GetAllAsync();
    }
}
