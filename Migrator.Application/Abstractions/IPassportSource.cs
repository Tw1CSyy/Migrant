using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Abstractions
{
    public interface IPassportSource
    {
        Task<IReadOnlyCollection<(string Series, string Number)>> GetPassportsAsync(
            CancellationToken ct);
    }
}
