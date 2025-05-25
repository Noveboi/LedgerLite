using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OccursAt_DateTimeToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OccursAtUtc",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OccursAt",
                schema: "Accounting",
                table: "JournalEntries",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                schema: "Accounting",
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 4,
                column: "Name",
                value: "Revenue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OccursAt",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.AddColumn<DateTime>(
                name: "OccursAtUtc",
                schema: "Accounting",
                table: "JournalEntries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                schema: "Accounting",
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 4,
                column: "Name",
                value: "Income");
        }
    }
}
