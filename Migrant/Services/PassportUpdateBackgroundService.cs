using Microsoft.Extensions.Options;
using Migrant.Application.Abstractions;
using Migrant.Application.Options;

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

                await RunOnce(stoppingToken);
            }
        }

        private async Task RunOnce(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();

            var runner = scope.ServiceProvider
                .GetRequiredService<IPassportUpdateRunner>();

            await runner.RunAsync(ct);
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
