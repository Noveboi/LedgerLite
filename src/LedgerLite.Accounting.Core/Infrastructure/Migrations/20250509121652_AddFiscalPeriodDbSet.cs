using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalPeriodDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_FiscalPeriod_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FiscalPeriod",
                schema: "Accounting",
                table: "FiscalPeriod");

            migrationBuilder.RenameTable(
                name: "FiscalPeriod",
                schema: "Accounting",
                newName: "FiscalPeriods",
                newSchema: "Accounting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FiscalPeriods",
                schema: "Accounting",
                table: "FiscalPeriods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_FiscalPeriods_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "FiscalPeriodId",
                principalSchema: "Accounting",
                principalTable: "FiscalPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_FiscalPeriods_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FiscalPeriods",
                schema: "Accounting",
                table: "FiscalPeriods");

            migrationBuilder.RenameTable(
                name: "FiscalPeriods",
                schema: "Accounting",
                newName: "FiscalPeriod",
                newSchema: "Accounting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FiscalPeriod",
                schema: "Accounting",
                table: "FiscalPeriod",
                column: "Id");

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
    }
}
