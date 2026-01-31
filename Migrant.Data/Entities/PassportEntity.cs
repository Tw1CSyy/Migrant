using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Data.Entities
{
    /// <summary>
    /// Сущность паспорта
    /// </summary>
    
    [Table("Passports")]
    public class PassportEntity
    {
        /// <summary>
        /// Идентификатор паспорта
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
        /// true - Пасспорт в списке МВД; в противном случае false
        /// </summary>
        public bool IsInactive { get; set; }

        /// <summary>
        /// Дата добавления паспорта
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата обновления паспорта
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
