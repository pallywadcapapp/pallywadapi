using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userprof : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_AspNetUsers_memberid",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_memberid",
                table: "UserProfile");

            migrationBuilder.RenameColumn(
                name: "ceated_date",
                table: "UserProfile",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "ceated_date",
                table: "MemberAccount",
                newName: "created_date");

            migrationBuilder.AlterColumn<string>(
                name: "memberid",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dob",
                table: "UserProfile",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "sex",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "bvn",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "dob",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "bvn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "dob",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "UserProfile",
                newName: "ceated_date");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "MemberAccount",
                newName: "ceated_date");

            migrationBuilder.AlterColumn<string>(
                name: "memberid",
                table: "UserProfile",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "dob",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sex",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_memberid",
                table: "UserProfile",
                column: "memberid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_AspNetUsers_memberid",
                table: "UserProfile",
                column: "memberid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
