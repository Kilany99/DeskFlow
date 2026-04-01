using DeskFlow.Application.Common;
using DeskFlow.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeskFlow.Server.Controllers.Base
{
    /// <summary>
    /// Serves as an abstract base class for API controllers, providing common functionality for accessing user and
    /// tenant information and handling API operation results.
    /// </summary>
    /// <remarks>Inherit from this class to simplify the implementation of API controllers that require access
    /// to the current user's identity, tenant context, or standardized result handling. This class exposes protected
    /// properties for retrieving the current user's ID, tenant ID, and role from the claims principal, and provides a
    /// method to generate appropriate HTTP responses based on operation results. This approach promotes consistency and
    /// reduces boilerplate code across API controllers.</remarks>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        protected Guid CurrentTenantId =>
            Guid.Parse(User.FindFirstValue("tenantId")!);

        protected string CurrentUserRole =>
            User.FindFirstValue(ClaimTypes.Role)!;

        protected IActionResult HandleResult<T>(Result<T> result) => result.StatusCode switch
        {
            200 => Ok(result.Data),
            201 => Created(string.Empty, result.Data),
            400 => BadRequest(new { error = result.Error }),
            401 => Unauthorized(new { error = result.Error }),
            403 => Forbid(),
            404 => NotFound(new { error = result.Error }),
            _ => StatusCode(result.StatusCode, new { error = result.Error })
        };
    }
}
