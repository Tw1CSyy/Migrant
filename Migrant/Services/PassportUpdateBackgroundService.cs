using Cronos;
using Microsoft.Extensions.Options;
using Migrant.Application.Abstractions;
using Migrant.Application.Options;
using System;

namespace Migrant.Services
{
    /// <summary>
    /// Фоновый сервис для обновление базы данных по времени
    /// </summary>
    public class PassportUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CronExpression _cron;

        public PassportUpdateBackgroundService(
            IServiceScopeFactory scopeFactory,
            IOptions<PassportUpdateOptions> options)
        {
            _scopeFactory = scopeFactory;
            _cron = CronExpression.Parse(options.Value.RunAt);
        }

        /// <summary>
        /// Основной цикл фонового сервиса.
        /// </summary>
        /// <param name="stoppingToken">Токен отмены, сигнализирующий о завершении работы приложения</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var next = _cron.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);

                if (next == null)
                    continue;

                var delay = next.Value - DateTimeOffset.Now;

                if (delay.TotalMilliseconds > 0)
                    await Task.Delay(delay, stoppingToken);

                await RunOnce(stoppingToken);
            }
        }

        /// <summary>
        /// Выполняет однократный запуск процесса обновления данных. Создаёт отдельный DI-scope и вызывает Application-сервис,
        /// </summary>
        /// <param name="ct">Токен отмены операции.</param>
        private async Task RunOnce(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();

            var runner = scope.ServiceProvider
                .GetRequiredService<IPassportUpdateRunner>();

            await runner.RunAsync(ct);
        }
    }
}
