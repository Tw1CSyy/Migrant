using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    /// <summary>
    /// Модель для вывода статуса паспорта
    /// </summary>
    public class PassportStatusDto
    {
        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string Series { get; set; } = null!;

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string Number { get; set; } = null!;

        /// <summary>
        /// Статус паспорта
        /// </summary>
        public bool IsInactive { get; set; }
    }
}
