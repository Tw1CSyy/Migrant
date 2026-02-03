using Microsoft.Extensions.Options;
using Migrant.Application.Abstractions;
using Migrant.Application.Options;

namespace Migrant.Services
{
    /// <summary>
    /// Фоновый сервис для обновление базы данных по времени
    /// </summary>
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

        /// <summary>
        /// Основной цикл фонового сервиса.
        /// </summary>
        /// <param name="stoppingToken">Токен отмены, сигнализирующий о завершении работы приложения</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = GetDelayUntilNextRun();
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

        /// <summary>
        /// Вычисляет интервал времени до следующего запуска обновления на основе времени, заданного в конфигурации.
        /// </summary>
        /// <returns>Временной интервал до следующего запуска фонового обновления.</returns>
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
