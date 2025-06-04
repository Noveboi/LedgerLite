using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedgerLite.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_OrganizationMember_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OrganizationMember_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationMember_Organizations_OrganizationId",
                schema: "Users",
                table: "OrganizationMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationMember",
                schema: "Users",
                table: "OrganizationMember");

            migrationBuilder.RenameTable(
                name: "OrganizationMember",
                schema: "Users",
                newName: "OrganizationMembers",
                newSchema: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationMember_OrganizationId",
                schema: "Users",
                table: "OrganizationMembers",
                newName: "IX_OrganizationMembers_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationMembers",
                schema: "Users",
                table: "OrganizationMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_OrganizationMembers_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "OrganizationMemberId",
                principalSchema: "Users",
                principalTable: "OrganizationMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OrganizationMembers_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers",
                column: "OrganizationMemberId",
                principalSchema: "Users",
                principalTable: "OrganizationMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationMembers_Organizations_OrganizationId",
                schema: "Users",
                table: "OrganizationMembers",
                column: "OrganizationId",
                principalSchema: "Users",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_OrganizationMembers_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OrganizationMembers_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationMembers_Organizations_OrganizationId",
                schema: "Users",
                table: "OrganizationMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationMembers",
                schema: "Users",
                table: "OrganizationMembers");

            migrationBuilder.RenameTable(
                name: "OrganizationMembers",
                schema: "Users",
                newName: "OrganizationMember",
                newSchema: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationMembers_OrganizationId",
                schema: "Users",
                table: "OrganizationMember",
                newName: "IX_OrganizationMember_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationMember",
                schema: "Users",
                table: "OrganizationMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_OrganizationMember_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "OrganizationMemberId",
                principalSchema: "Users",
                principalTable: "OrganizationMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OrganizationMember_OrganizationMemberId",
                schema: "Users",
                table: "AspNetUsers",
                column: "OrganizationMemberId",
                principalSchema: "Users",
                principalTable: "OrganizationMember",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationMember_Organizations_OrganizationId",
                schema: "Users",
                table: "OrganizationMember",
                column: "OrganizationId",
                principalSchema: "Users",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
