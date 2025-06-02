using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LedgerLite.Accounting.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounting");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "character varying(6)", unicode: false, maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    IsPlaceholder = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountType",
                schema: "Accounting",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Charts",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "Accounting",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriods",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryStatus",
                schema: "Accounting",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryStatus", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryType",
                schema: "Accounting",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryType", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "AccountNode",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChartId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountNode_AccountNode_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Accounting",
                        principalTable: "AccountNode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountNode_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountNode_Charts_ChartId",
                        column: x => x.ChartId,
                        principalSchema: "Accounting",
                        principalTable: "Charts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FiscalPeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OccursAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_FiscalPeriods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalSchema: "Accounting",
                        principalTable: "FiscalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLines",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_JournalEntries_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Accounting",
                table: "AccountType",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 1, "Asset" },
                    { 2, "Liability" },
                    { 3, "Expense" },
                    { 4, "Revenue" },
                    { 5, "Equity" }
                });

            migrationBuilder.InsertData(
                schema: "Accounting",
                table: "Currency",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 1, "EUR" },
                    { 2, "USD" },
                    { 3, "GBP" }
                });

            migrationBuilder.InsertData(
                schema: "Accounting",
                table: "JournalEntryStatus",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 1, "Editable" },
                    { 2, "Posted" },
                    { 3, "Reversed" }
                });

            migrationBuilder.InsertData(
                schema: "Accounting",
                table: "JournalEntryType",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 1, "Standard" },
                    { 2, "Recurring" },
                    { 3, "Adjusting" },
                    { 4, "Reversing" },
                    { 5, "Compound" },
                    { 6, "Opening" },
                    { 7, "Closing" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountNode_AccountId",
                schema: "Accounting",
                table: "AccountNode",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountNode_ChartId",
                schema: "Accounting",
                table: "AccountNode",
                column: "ChartId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountNode_ParentId",
                schema: "Accounting",
                table: "AccountNode",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_OrganizationId",
                schema: "Accounting",
                table: "Charts",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriods_OrganizationId",
                schema: "Accounting",
                table: "FiscalPeriods",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_FiscalPeriodId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId",
                schema: "Accounting",
                table: "JournalEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_EntryId",
                schema: "Accounting",
                table: "JournalEntryLines",
                column: "EntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountNode",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "AccountType",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "JournalEntryLines",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "JournalEntryStatus",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "JournalEntryType",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Charts",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "FiscalPeriods",
                schema: "Accounting");
        }
    }
}
