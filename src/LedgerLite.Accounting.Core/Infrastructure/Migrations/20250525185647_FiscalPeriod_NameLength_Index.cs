using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FiscalPeriod_NameLength_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Accounting",
                table: "FiscalPeriods",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriods_OrganizationId",
                schema: "Accounting",
                table: "FiscalPeriods",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FiscalPeriods_OrganizationId",
                schema: "Accounting",
                table: "FiscalPeriods");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Accounting",
                table: "FiscalPeriods");
        }
    }
}
