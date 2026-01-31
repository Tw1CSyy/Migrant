using Microsoft.Extensions.DependencyInjection;
using Migrant.Application.Services;

namespace Migrant.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует все сервисы Application-слоя
        /// </summary>
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<PassportUpdateService>();
            services.AddScoped<PassportQueryService>();
            services.AddScoped<PassportHistoryService>();

            return services;
        }
    }
}
