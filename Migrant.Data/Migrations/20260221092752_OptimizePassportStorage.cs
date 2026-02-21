using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrant.Data.Migrations
{
    /// <inheritdoc />
    public partial class OptimizePassportStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Композитный PK (series, number)
            
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_constraint
                        WHERE conname = 'PK_Passports'
                    ) THEN
                        ALTER TABLE ""Passports"" DROP CONSTRAINT ""PK_Passports"";
                    END IF;
                END $$;
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Passports""
                ADD CONSTRAINT pk_passports PRIMARY KEY (""Series"", ""Number"");
                ");

            // 2. Индекс для изменений по дате
            
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS ix_passport_changes_date
                ON ""PassportChanges"" (""ChangeDate"");
                ");

            // 3. Индекс для истории

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS ix_passport_history_key
                ON ""PassportHistory""(""Series"", ""Number"");
                ");

            // 4. Staging таблица (UNLOGGED)

            migrationBuilder.Sql(@"
                CREATE UNLOGGED TABLE IF NOT EXISTS passport_staging (
                    Series text NOT NULL,
                    Number text NOT NULL
                );
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаляем staging
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS passport_staging;
                ");

            // Удаляем индексы
            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS ix_passport_changes_date;
                ");

            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS ix_passport_history_key;
                ");

            // Возврат к PK по Id (если нужен)
            migrationBuilder.Sql(@"
                ALTER TABLE passports DROP CONSTRAINT IF EXISTS pk_passports;
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE passports
                ADD CONSTRAINT pk_passports PRIMARY KEY (id);
                ");

        }
    }
}
