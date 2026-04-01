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
    }
}
