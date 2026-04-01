using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Services.Interfaces.Base
{
    public interface IActivatableService
    {
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);
    }

}
