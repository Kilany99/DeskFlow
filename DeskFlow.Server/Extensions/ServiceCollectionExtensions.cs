using DeskFlow.Application.Services.Implementation;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Interfaces;
using DeskFlow.Infrastructure.Data;
using DeskFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            return services;
        }

    }
}
