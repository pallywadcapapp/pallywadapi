using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAccount_AspNetUsers_memberid",
                table: "MemberAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAccount",
                table: "MemberAccount");

            migrationBuilder.RenameTable(
                name: "MemberAccount",
                newName: "MemberAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_MemberAccount_memberid",
                table: "MemberAccounts",
                newName: "IX_MemberAccounts_memberid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAccounts",
                table: "MemberAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAccounts_AspNetUsers_memberid",
                table: "MemberAccounts",
                column: "memberid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAccounts_AspNetUsers_memberid",
                table: "MemberAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAccounts",
                table: "MemberAccounts");

            migrationBuilder.RenameTable(
                name: "MemberAccounts",
                newName: "MemberAccount");

            migrationBuilder.RenameIndex(
                name: "IX_MemberAccounts_memberid",
                table: "MemberAccount",
                newName: "IX_MemberAccount_memberid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAccount",
                table: "MemberAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAccount_AspNetUsers_memberid",
                table: "MemberAccount",
                column: "memberid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
