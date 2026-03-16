using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrant.Application.Abstractions;
using Migrant.Data.Abstractions;
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
        private readonly IPassportImportService _import;
        private readonly IPassportMergeService _merge;

        public PassportUpdateRunner(
            IPassportSource source,
            IServiceProvider provider,
            IConfiguration config,
            IPassportMergeService merge,
            IPassportImportService import)
        {
            _source = source;
            _provider = provider;
            _config = config;
            _merge = merge;
            _import = import;
        }

        /// <summary>
        /// Запуск обновление базы данных
        /// </summary>
        public async Task RunAsync(CancellationToken ct, Stream stream = null)
        {
            if(stream == null)
                stream = await _source.GetFileStreamAsync(ct);

            using var scope = _provider.CreateScope();

            await _import.ImportAsync(stream, ct);
            await _merge.MergeAsync(ct);
        }
    }
}
