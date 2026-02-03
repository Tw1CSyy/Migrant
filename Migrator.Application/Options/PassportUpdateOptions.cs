using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Options
{
    /// <summary>
    /// Модель конфигурационных данных
    /// </summary>
    public class PassportUpdateOptions
    {
        /// <summary>
        /// Время автоматического обновления
        /// </summary>
        public string RunAt { get; set; } = null!;

        /// <summary>
        /// Путь до файла исходных данных
        /// </summary>
        public string SourcePath { get; set; } = null!;

        /// <summary>
        /// URL до файла исходных данных
        /// </summary>
        public string SourceUrl { get; set; } = null!;
    }
}
