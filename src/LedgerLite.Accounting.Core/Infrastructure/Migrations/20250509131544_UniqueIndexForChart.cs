using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexForChart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Charts_OrganizationId",
                schema: "Accounting",
                table: "Charts",
                column: "OrganizationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Charts_OrganizationId",
                schema: "Accounting",
                table: "Charts");
        }
    }
}
