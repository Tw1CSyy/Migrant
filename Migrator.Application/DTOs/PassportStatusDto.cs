using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    public class PassportStatusDto
    {
        public string Series { get; set; } = null!;
        public string Number { get; set; } = null!;
        public bool IsInactive { get; set; }
    }
}
