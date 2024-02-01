using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class foreignId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAccounts_AspNetUsers_memberid",
                table: "MemberAccounts");

            migrationBuilder.DropIndex(
                name: "IX_MemberAccounts_memberid",
                table: "MemberAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "memberid",
                table: "MemberAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppIdentityUserId",
                table: "MemberAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAccounts_AppIdentityUserId",
                table: "MemberAccounts",
                column: "AppIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAccounts_AspNetUsers_AppIdentityUserId",
                table: "MemberAccounts",
                column: "AppIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAccounts_AspNetUsers_AppIdentityUserId",
                table: "MemberAccounts");

            migrationBuilder.DropIndex(
                name: "IX_MemberAccounts_AppIdentityUserId",
                table: "MemberAccounts");

            migrationBuilder.DropColumn(
                name: "AppIdentityUserId",
                table: "MemberAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "memberid",
                table: "MemberAccounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAccounts_memberid",
                table: "MemberAccounts",
                column: "memberid");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAccounts_AspNetUsers_memberid",
                table: "MemberAccounts",
                column: "memberid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
