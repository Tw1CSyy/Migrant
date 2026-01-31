using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrant.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PassportChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Series = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ChangeType = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassportHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Series = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsInactive = table.Column<bool>(type: "boolean", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Series = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsInactive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassportChanges_ChangeDate",
                table: "PassportChanges",
                column: "ChangeDate");

            migrationBuilder.CreateIndex(
                name: "IX_PassportHistory_Series_Number",
                table: "PassportHistory",
                columns: new[] { "Series", "Number" });

            migrationBuilder.CreateIndex(
                name: "IX_Passports_IsInactive",
                table: "Passports",
                column: "IsInactive");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_Series_Number",
                table: "Passports",
                columns: new[] { "Series", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassportChanges");

            migrationBuilder.DropTable(
                name: "PassportHistory");

            migrationBuilder.DropTable(
                name: "Passports");
        }
    }
}
