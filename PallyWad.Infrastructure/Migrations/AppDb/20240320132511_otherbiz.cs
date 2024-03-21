using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class otherbiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "business_email",
                table: "BusinessInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "business_phoneNo",
                table: "BusinessInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sector",
                table: "BusinessInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "services",
                table: "BusinessInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tin",
                table: "BusinessInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_email",
                table: "BusinessInformation");

            migrationBuilder.DropColumn(
                name: "business_phoneNo",
                table: "BusinessInformation");

            migrationBuilder.DropColumn(
                name: "sector",
                table: "BusinessInformation");

            migrationBuilder.DropColumn(
                name: "services",
                table: "BusinessInformation");

            migrationBuilder.DropColumn(
                name: "tin",
                table: "BusinessInformation");
        }
    }
}
