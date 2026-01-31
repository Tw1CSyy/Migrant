using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrant.Data.Context;

namespace Migrant.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует DbContext Data-слоя
        /// </summary>
        public static IServiceCollection AddData(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PassportDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));

            return services;
        }
    }
}
