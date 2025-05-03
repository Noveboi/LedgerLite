using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequireEnumerationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TransactionType",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JournalEntryType",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JournalEntryStatus",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Currency",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccountType",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 1,
                column: "Name",
                value: "Asset");

            migrationBuilder.UpdateData(
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 2,
                column: "Name",
                value: "Liability");

            migrationBuilder.UpdateData(
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 3,
                column: "Name",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 4,
                column: "Name",
                value: "Income");

            migrationBuilder.UpdateData(
                table: "AccountType",
                keyColumn: "Value",
                keyValue: 5,
                column: "Name",
                value: "Equity");

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Value",
                keyValue: 1,
                column: "Name",
                value: "EUR");

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Value",
                keyValue: 2,
                column: "Name",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Value",
                keyValue: 3,
                column: "Name",
                value: "GBP");

            migrationBuilder.UpdateData(
                table: "JournalEntryStatus",
                keyColumn: "Value",
                keyValue: 1,
                column: "Name",
                value: "Editable");

            migrationBuilder.UpdateData(
                table: "JournalEntryStatus",
                keyColumn: "Value",
                keyValue: 2,
                column: "Name",
                value: "Posted");

            migrationBuilder.UpdateData(
                table: "JournalEntryStatus",
                keyColumn: "Value",
                keyValue: 3,
                column: "Name",
                value: "Reversed");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 1,
                column: "Name",
                value: "Standard");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 2,
                column: "Name",
                value: "Recurring");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 3,
                column: "Name",
                value: "Adjusting");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 4,
                column: "Name",
                value: "Reversing");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 5,
                column: "Name",
                value: "Compound");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 6,
                column: "Name",
                value: "Opening");

            migrationBuilder.UpdateData(
                table: "JournalEntryType",
                keyColumn: "Value",
                keyValue: 7,
                column: "Name",
                value: "Closing");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Value",
                keyValue: 1,
                column: "Name",
                value: "Credit");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Value",
                keyValue: 2,
                column: "Name",
                value: "Debit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TransactionType");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "JournalEntryType");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "JournalEntryStatus");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccountType");
        }
    }
}
