using Microsoft.Extensions.DependencyInjection;
using Migrant.Application.Abstractions;
using Migrant.Application.Services;
using Migrant.Data.Abstractions;
using Migrant.Data.Services;

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
            services.AddScoped<PassportQueryService>();
            services.AddScoped<PassportFileSource>();
            services.AddScoped<IPassportSource, PassportFileSource>();
            services.AddScoped<IPassportUpdateRunner, PassportUpdateRunner>();
            services.AddScoped<PassportQueryService>();
            services.AddScoped<IPassportImportService, PassportImportService>();
            services.AddScoped<IPassportMergeService, PassportMergeService>();
            return services;
        }
    }
}
