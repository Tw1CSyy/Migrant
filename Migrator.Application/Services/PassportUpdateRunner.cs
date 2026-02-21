using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrant.Application.Abstractions;
using Migrant.Data.Services;

namespace Migrant.Application.Services
{
    /// <summary>
    /// Сервис для запуска обновления базы данных
    /// </summary>
    public class PassportUpdateRunner : IPassportUpdateRunner
    {
        private readonly IPassportSource _source;
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;

        public PassportUpdateRunner(
            IPassportSource source,
            IServiceProvider provider,
            IConfiguration config)
        {
            _source = source;
            _provider = provider;
            _config = config;
        }

        /// <summary>
        /// Запуск обновление базы данных
        /// </summary>
        public async Task RunAsync(CancellationToken ct)
        {
            var stream = await _source.GetFileStreamAsync(ct);

            using var scope = _provider.CreateScope();

            var importer = scope.ServiceProvider.GetRequiredService<PassportImportService>();
            var merger = scope.ServiceProvider.GetRequiredService<PassportMergeService>();

            await importer.ImportAsync(stream, ct);
            await merger.MergeAsync(ct);
        }
    }
}
