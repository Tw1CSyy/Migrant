using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Data.Services
{
    public class PassportImportService
    {
        private readonly string _connectionString;

        public PassportImportService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Postgres")!;
        }

        public async Task ImportAsync(Stream zipStream, CancellationToken ct)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(ct);

            // очищаем staging
            await using (var truncate = new NpgsqlCommand(
                "TRUNCATE passport_staging", conn))
            {
                await truncate.ExecuteNonQueryAsync(ct);
            }

            using var archive = new ZipArchive(zipStream);
            var entry = archive.Entries.First();

            await using var csvStream = entry.Open();
            using var reader = new StreamReader(csvStream);

            await using var writer = conn.BeginTextImport(
                "COPY passport_staging (series, number) FROM STDIN (FORMAT csv)");

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
               await writer.WriteLineAsync(line);
            }
        }
    }
}
