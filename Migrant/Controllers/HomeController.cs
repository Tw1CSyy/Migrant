using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Migrant.Application.Abstractions;
using Migrant.Application.Services;
using Migrant.Services;

namespace Migrant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IPassportUpdateRunner _runner;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PassportQueryService _queryService;

        public HomeController(IPassportUpdateRunner runner, IServiceScopeFactory scopeFactory, PassportQueryService queryService)
        {
            _runner = runner;
            _scopeFactory = scopeFactory;
            _queryService = queryService;
        }

        [HttpPost("update")]
        public IActionResult Update(CancellationToken ct)
        {
            Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var runner = scope.ServiceProvider
                    .GetRequiredService<IPassportUpdateRunner>();

                await runner.RunAsync(CancellationToken.None);
            });

            return Accepted("Update started");
        }

        /// <summary>
        /// Поиск неактивного паспорта
        /// </summary>
        /// <param name="series">Серия паспорта</param>
        /// <param name="number">Номер паспорта</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search(
        [FromQuery] string series,
        [FromQuery] string number,
        CancellationToken ct)
        {
            var result = await _queryService.FindAsync(series, number, ct);
            return Ok(result);
        }

        /// <summary>
        /// Получение изменений за дату
        /// </summary>
        /// <param name="date">Дата, для которой ищем изменения</param>
        [HttpGet("changes")]
        public async Task<IActionResult> Changes([FromQuery] DateTime date, CancellationToken ct)
        {
            var result = await _queryService.GetChangesAsync(date, ct);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        /// <summary>
        /// Возвращает данные истории активности / неактивности паспорта
        /// </summary>
        /// <param name="series">Серия паспорта</param>
        /// <param name="number">Номер паспорта</param>
        [HttpGet("history")]
        public async Task<IActionResult> History([FromQuery] string series, [FromQuery] string number, CancellationToken ct)
        {
            var result = await _queryService.GetHistoryAsync(series, number, ct);

            return result is null
                ? NotFound()
                : Ok(result);
        }
    }
}
