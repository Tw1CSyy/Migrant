using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Data.Entities
{
    /// <summary>
    /// Сущность истории активности
    /// </summary>
    
    [Table("PassportHistory")]
    public class PassportStatusHistoryEntity
    {
        /// <summary>
        /// Идентификатор записи в истории
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string Series { get; set; } = null!;

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string Number { get; set; } = null!;

        /// <summary>
        /// Активен ли паспорт
        /// </summary>
        public bool IsInactive { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ChangedAt { get; set; }
    }
}
