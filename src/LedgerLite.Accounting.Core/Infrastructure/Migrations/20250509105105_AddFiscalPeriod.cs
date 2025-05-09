using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                schema: "Accounting",
                table: "JournalEntries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedByUserId",
                schema: "Accounting",
                table: "JournalEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FiscalPeriod",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "FiscalPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_FiscalPeriod_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "FiscalPeriodId",
                principalSchema: "Accounting",
                principalTable: "FiscalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_FiscalPeriod_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropTable(
                name: "FiscalPeriod",
                schema: "Accounting");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                schema: "Accounting",
                table: "JournalEntries");
        }
    }
}
