using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Data.Entities
{
    /// <summary>
    /// Сущность изменения за дату
    /// </summary>

    [Table("PassportChanges")]
    public class PassportChangeEntity
    {
        /// <summary>
        /// Идентификатор изменения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string Series { get; set; } = null!;

        /// <summary>
        /// Номер пасспорта
        /// </summary>
        public string Number { get; set; } = null!;

        /// <summary>
        /// Тип изменения
        /// </summary>
        public PassportChangeType ChangeType { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ChangeDate { get; set; }
    }

    /// <summary>
    /// Тип изменения паспорта
    /// </summary>
    public enum PassportChangeType
    {
        /// <summary>
        /// Паспорт добавлен
        /// </summary>
        Added = 1,

        /// <summary>
        /// Паспорт удален
        /// </summary>
        Removed = 2
    }
}
