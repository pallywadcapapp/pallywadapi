using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class lrpurpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loanBal",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "netBal",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "savBal",
                table: "LoanRequests");

            migrationBuilder.AddColumn<string>(
                name: "subject",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "admiDocumentRef",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "age",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "businessname",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "collateral",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "collaterizedDate",
                table: "LoanRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isCollateralReceived",
                table: "LoanRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDocmentProvided",
                table: "LoanRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isProcessCleared",
                table: "LoanRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "preferredRate",
                table: "LoanRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "processedDate",
                table: "LoanRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "purpose",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sector",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subject",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "admiDocumentRef",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "age",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "businessname",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "collateral",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "collaterizedDate",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "isCollateralReceived",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "isDocmentProvided",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "isProcessCleared",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "preferredRate",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "processedDate",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "purpose",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "sector",
                table: "LoanRequests");

            migrationBuilder.AddColumn<double>(
                name: "loanBal",
                table: "LoanRequests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "netBal",
                table: "LoanRequests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "savBal",
                table: "LoanRequests",
                type: "float",
                nullable: true);
        }
    }
}
