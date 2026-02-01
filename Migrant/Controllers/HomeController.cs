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

        public HomeController(IPassportUpdateRunner runner, IServiceScopeFactory scopeFactory)
        {
            _runner = runner;
            _scopeFactory = scopeFactory;
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
    }
}
