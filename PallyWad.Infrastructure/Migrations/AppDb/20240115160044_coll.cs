using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class coll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guarantorId2",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "guarantorId3",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "othercoorp",
                table: "LoanRequests");

            migrationBuilder.RenameColumn(
                name: "guarantorId1",
                table: "LoanRequests",
                newName: "collateralId");

            migrationBuilder.AddColumn<string>(
                name: "otherdetails",
                table: "UserCollaterals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "otherdetails",
                table: "UserCollaterals");

            migrationBuilder.RenameColumn(
                name: "collateralId",
                table: "LoanRequests",
                newName: "guarantorId1");

            migrationBuilder.AddColumn<string>(
                name: "guarantorId2",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guarantorId3",
                table: "LoanRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "othercoorp",
                table: "LoanRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
