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

                // ВАЖНО: отдельный using-блок
                await using (var importer = conn.BeginTextImport(
                    @"COPY public.""passport_staging""(""series"", ""number"") 
                    FROM STDIN (FORMAT csv, HEADER true)"))
                {
                    var importStream = ((StreamWriter)importer).BaseStream;

                    await csvStream.CopyToAsync(importStream, 1024 * 1024, ct);
                    await importStream.FlushAsync(ct);
                } // ← importer.Dispose() происходит здесь

                // Теперь соединение уже не в Copy state
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
