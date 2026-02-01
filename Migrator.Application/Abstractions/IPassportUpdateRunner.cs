using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Abstractions
{
    public interface IPassportUpdateRunner
    {
        Task RunAsync(CancellationToken ct);
    }
}
