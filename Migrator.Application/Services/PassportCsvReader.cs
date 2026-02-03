using CsvHelper;
using Migrant.Application.Options;
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
        public async Task<List<PassportKey>> ReadAsync(Stream csv)
        {
            using var reader = new StreamReader(csv);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var result = new List<PassportKey>();

            await foreach (var record in csvReader.GetRecordsAsync<dynamic>())
            {
                string series = record.PASSP_SERIES;
                string number = record.PASSP_NUMBER;

                result.Add(new PassportKey(series, number));
            }

            return result;
        }
    }
}
