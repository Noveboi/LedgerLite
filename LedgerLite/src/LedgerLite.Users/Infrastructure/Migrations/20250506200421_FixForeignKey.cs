using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationMember_AspNetUsers_OrganizationId",
                schema: "Users",
                table: "OrganizationMember");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationMember_OrganizationId",
                schema: "Users",
                table: "OrganizationMember");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_OrganizationId",
                schema: "Users",
                table: "OrganizationMember",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers",
                column: "OrganizationMemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OrganizationMember_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers",
                column: "OrganizationMemberId",
                principalSchema: "Users",
                principalTable: "OrganizationMember",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OrganizationMember_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationMember_OrganizationId",
                schema: "Users",
                table: "OrganizationMember");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_OrganizationId",
                schema: "Users",
                table: "OrganizationMember",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationMember_AspNetUsers_OrganizationId",
                schema: "Users",
                table: "OrganizationMember",
                column: "OrganizationId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
