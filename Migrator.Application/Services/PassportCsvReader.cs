using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Application.Services
{
    public class PassportCsvReader
    {
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
