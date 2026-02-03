using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.DTOs
{
    /// <summary>
    /// Модель для вывода изменений паспортов
    /// </summary>
    public class PassportChangesDto
    {
        /// <summary>
        /// Дата, за которую были зафиксированы изменения
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Список паспортов, которые были добавлены
        /// </summary>
        public List<string> Added { get; set; } = new();

        /// <summary>
        /// Список паспортов, которые были удалены
        /// </summary>
        public List<string> Removed { get; set; } = new();
    }
}
