using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    /// <summary>
    /// Модель для вывода истории изменения паспота
    /// </summary>
    public class PassportHistoryDto
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
        /// Список изменений по паспорту
        /// </summary>
        public List<PassportHistoryItemDto> History { get; set; } = new();
    }

    /// <summary>
    /// Модель для вывода записи в истории паспорта
    /// </summary>
    public class PassportHistoryItemDto
    {
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Значение, которые было поставлено
        /// </summary>
        public bool IsInactive { get; set; }
    }
}
