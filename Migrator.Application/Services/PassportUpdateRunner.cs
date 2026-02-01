using Migrant.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Services
{
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
