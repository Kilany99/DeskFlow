using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces.Base
{
    public interface IDeletableService
    {
        Task DeleteAsync(Guid id);
    }
}
