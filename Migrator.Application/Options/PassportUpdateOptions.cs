using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Options
{
    public class PassportUpdateOptions
    {
        public string RunAt { get; set; } = null!;
        public string SourcePath { get; set; } = null!;
        public string SourceUrl { get; set; } = null!;
    }
}
