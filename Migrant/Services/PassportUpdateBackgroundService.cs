using Microsoft.Extensions.Options;
using Migrant.Options;
using Migrant.Application.Services;

namespace Migrant.Services
{
    public class PassportUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PassportUpdateOptions _options;

        public PassportUpdateBackgroundService(
            IServiceScopeFactory scopeFactory,
            IOptions<PassportUpdateOptions> options)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = GetDelayUntilNextRun();
                await Task.Delay(delay, stoppingToken);

                await RunUpdate(stoppingToken);
            }
        }

        private async Task RunUpdate(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();

            var downloader = scope.ServiceProvider.GetRequiredService<PassportFileDownloader>();
            var extractor = scope.ServiceProvider.GetRequiredService<ZipExtractor>();
            var reader = scope.ServiceProvider.GetRequiredService<PassportCsvReader>();
            var updater = scope.ServiceProvider.GetRequiredService<PassportUpdateService>();

            using var zip = await downloader.DownloadAsync(_options.SourceUrl, ct);
            using var csv = await extractor.ExtractCsvAsync(zip);

            var passports = await reader.ReadAsync(csv);

            await updater.UpdateAsync(passports, DateTime.UtcNow, ct);
        }

        private TimeSpan GetDelayUntilNextRun()
        {
            var runAt = TimeSpan.Parse(_options.RunAt);
            var now = DateTime.Now.TimeOfDay;

            return now < runAt
                ? runAt - now
                : TimeSpan.FromDays(1) - (now - runAt);
        }
    }
}
