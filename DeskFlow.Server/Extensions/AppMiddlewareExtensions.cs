using DeskFlow.Server.Middleware;

namespace DeskFlow.Server.Extensions
{
    public static class AppMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenantResolution(
            this IApplicationBuilder app)
            => app.UseMiddleware<TenantResolutionMiddleware>();
    }
}
