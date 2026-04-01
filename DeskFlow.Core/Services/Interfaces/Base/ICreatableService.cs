using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces.Base
{
    public interface ICreatableService<TRequest, TResponse>
    {
        Task<TResponse> CreateAsync(TRequest request);
    }
}
