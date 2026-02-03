using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Services
{
    /// <summary>
    /// Сервис для обработки Csv файла
    /// </summary>
    public class PassportCsvReader
    {
        /// <summary>
        /// Обрабатывает Csv файл и возвращает список строк из файла
        /// </summary>
        /// <param name="csv">Stream файла</param>
        /// <returns>Task<List<(string Series, string Number)</returns>
        public async Task<List<(string Series, string Number)>> ReadAsync(Stream csv)
        {
            using var reader = new StreamReader(csv);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var result = new List<(string, string)>();

            await foreach (var record in csvReader.GetRecordsAsync<dynamic>())
            {
                string series = record.PASSP_SERIES;
                string number = record.PASSP_NUMBER;

                result.Add((series, number));
            }

            return result;
        }
    }
}
