using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureChartOrganizationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "Accounting",
                table: "Charts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "Accounting",
                table: "Charts");
        }
    }
}
