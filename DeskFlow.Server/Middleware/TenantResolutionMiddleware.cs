using DeskFlow.Domain.Interfaces;

namespace DeskFlow.Server.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext context, IUnitOfWork uow)
        {
            // Skip public endpoints that don't need tenant resolution
            if (context.Request.Path.StartsWithSegments("/api/auth"))
            {
                await _next(context);
                return;
            }

            var tenantId = ResolveTenantId(context);

            if (tenantId == Guid.Empty)
            {
                await _next(context);
                return;
            }

            // Validate tenant exists and is active
            var tenant = await uow.Tenants.GetByIdAsync(tenantId);

            if (tenant is null || !tenant.IsActive)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Tenant not found or suspended"
                });
                return;
            }

            // Store tenant in HttpContext for downstream use
            context.Items["Tenant"] = tenant;
            context.Items["TenantId"] = tenantId;

            await _next(context);
        }

        private static Guid ResolveTenantId(HttpContext context)
        {
            // 1. Try JWT claim first (authenticated requests)
            var tenantClaim = context.User.FindFirst("tenantId")?.Value;
            if (!string.IsNullOrEmpty(tenantClaim) && Guid.TryParse(tenantClaim, out var id))
                return id;

            // 2. Try X-Tenant-Id header (public endpoints like ticket submission)
            var header = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(header) && Guid.TryParse(header, out var headerId))
                return headerId;

            // 3. Try subdomain resolution (e.g. acme.deskflow.com)
            var host = context.Request.Host.Host;
            var subdomain = host.Split('.').FirstOrDefault();
            if (!string.IsNullOrEmpty(subdomain) && subdomain != "localhost")
                context.Items["Subdomain"] = subdomain;

            return Guid.Empty;
        }
    }
}
