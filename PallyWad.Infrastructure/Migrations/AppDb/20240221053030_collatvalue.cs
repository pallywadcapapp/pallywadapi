using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class collatvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "collateralValue",
                table: "LoanRequests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "estimatedCollateralValue",
                table: "LoanRequests",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "collateralValue",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "estimatedCollateralValue",
                table: "LoanRequests");
        }
    }
}
