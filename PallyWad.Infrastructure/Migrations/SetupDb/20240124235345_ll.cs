using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.SetupDb
{
    /// <inheritdoc />
    public partial class ll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "savingscode",
                table: "LoanSetups");

            migrationBuilder.DropColumn(
                name: "sharecode",
                table: "LoanSetups");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "LoanSetups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "collateralPercentage",
                table: "LoanSetups",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age",
                table: "LoanSetups");

            migrationBuilder.DropColumn(
                name: "collateralPercentage",
                table: "LoanSetups");

            migrationBuilder.DropColumn(
                name: "type",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "savingscode",
                table: "LoanSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sharecode",
                table: "LoanSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
