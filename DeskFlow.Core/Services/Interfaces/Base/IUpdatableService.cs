using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces.Base
{
    public interface IUpdatableService<TRequest, TResponse>
    {
        Task<TResponse> UpdateAsync(Guid id, TRequest request);
    }

}
