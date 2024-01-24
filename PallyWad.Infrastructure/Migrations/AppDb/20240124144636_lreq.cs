using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class lreq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanUserCollaterals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loanRequestId = table.Column<int>(type: "int", nullable: false),
                    userCollateralId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanUserCollaterals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanUserCollaterals_LoanRequests_loanRequestId",
                        column: x => x.loanRequestId,
                        principalTable: "LoanRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanUserDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loanRequestId = table.Column<int>(type: "int", nullable: false),
                    userDocumentlId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanUserDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanUserDocuments_LoanRequests_loanRequestId",
                        column: x => x.loanRequestId,
                        principalTable: "LoanRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanUserCollaterals_loanRequestId",
                table: "LoanUserCollaterals",
                column: "loanRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanUserDocuments_loanRequestId",
                table: "LoanUserDocuments",
                column: "loanRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanUserCollaterals");

            migrationBuilder.DropTable(
                name: "LoanUserDocuments");
        }
    }
}
