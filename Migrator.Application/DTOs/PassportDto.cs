using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    public record PassportDto (
    string Series,
    string Number,
    bool IsInactive
    );
}
