using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    public record PassportChangeDto(
    string Series,
    string Number,
    string ChangeType,
    DateTime ChangeDate
    );
}
