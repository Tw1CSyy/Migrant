using Migrant.Application.Abstractions;

namespace Migrant.Application.Services
{
    /// <summary>
    /// Сервис для запуска обновления базы данных
    /// </summary>
    public class PassportUpdateRunner : IPassportUpdateRunner
    {
        private readonly IPassportSource _source;
        private readonly PassportUpdateService _updateService;

        public PassportUpdateRunner(
            IPassportSource source,
            PassportUpdateService updateService)
        {
            _source = source;
            _updateService = updateService;
        }

        /// <summary>
        /// Запуск обновлени ябазы данных
        /// </summary>
        public async Task RunAsync(CancellationToken ct)
        {
            var passports = await _source.GetPassportsAsync(ct);

            await _updateService.UpdateAsync(
                passports,
                DateTime.UtcNow,
                ct);
        }
    }
}
