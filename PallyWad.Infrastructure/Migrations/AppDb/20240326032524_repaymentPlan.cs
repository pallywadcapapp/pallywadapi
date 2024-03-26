using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class repaymentPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "repaymentPlan",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "repaymentPlan",
                table: "LoanRequests");
        }
    }
}
