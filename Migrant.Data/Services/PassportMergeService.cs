using Microsoft.Extensions.Configuration;
using Migrant.Data.Abstractions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Data.Services
{
    public class PassportMergeService : IPassportMergeService
    {
        private readonly string _connectionString;

        public PassportMergeService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Postgres")!;
        }

        public async Task MergeAsync(CancellationToken ct)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(ct);

            await using var tx = await conn.BeginTransactionAsync(ct);

            var sql = @"
                WITH changes AS (
                    MERGE INTO ""Passports"" p
                    USING ""passport_staging"" s
                    ON p.""Series"" = s.""series"" AND p.""Number"" = s.""number""

                    WHEN NOT MATCHED THEN
                        INSERT (""Series"", ""Number"", ""IsInactive"", ""CreatedAt"", ""UpdatedAt"")
                        VALUES (s.""series"", s.""number"", true, now(), now())

                    WHEN NOT MATCHED BY SOURCE AND p.""IsInactive"" = true THEN
                        UPDATE SET 
                            ""IsInactive"" = false,
                            ""UpdatedAt"" = now()

                    RETURNING p.""Series"", p.""Number"", p.""IsInactive""
                )
                INSERT INTO ""PassportChanges""(""Series"", ""Number"", ""ChangeType"", ""ChangeDate"")
                SELECT 
                   ""Series"",
                   ""Number"",
                    CASE WHEN ""IsInactive"" THEN 'added' ELSE 'removed' END,
                    now()
                FROM changes;

                INSERT INTO ""PassportHistory""(""Series"", ""Number"", ""IsInactive"", ""ChangedAt"")
                SELECT 
                    ""Series"",
                    ""Number"",
                    ""IsInactive"",
                    now()
                FROM changes;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn, tx);
            await cmd.ExecuteNonQueryAsync(ct);

            await tx.CommitAsync(ct);
        }
    }
}
