using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO.Compression;

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

            await using var tx = await conn.BeginTransactionAsync(ct);

            try
            {
                await using (var truncate = new NpgsqlCommand(
                    @"TRUNCATE TABLE public.""passport_staging"";", conn, tx))
                {
                    await truncate.ExecuteNonQueryAsync(ct);
                }

                using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
                var entry = archive.Entries.Last();

                await using var csvStream = entry.Open();

                await using var importer = conn.BeginTextImport(
                 @"COPY public.""passport_staging""(""series"", ""number"")
                 FROM STDIN (FORMAT csv, HEADER true)");

                using var reader = new StreamReader(csvStream);

                char[] buffer = new char[1024 * 1024];

                int read;
                while ((read = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await importer.WriteAsync(buffer, 0, read);
                }

                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}
