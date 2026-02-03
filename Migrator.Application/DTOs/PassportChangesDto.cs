using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    public class PassportChangesDto
    {
        public DateTime Date { get; set; }
        public List<string> Added { get; set; } = new();
        public List<string> Removed { get; set; } = new();
    }
}
