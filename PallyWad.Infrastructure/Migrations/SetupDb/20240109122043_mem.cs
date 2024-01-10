using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.SetupDb
{
    /// <inheritdoc />
    public partial class mem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ceated_date",
                table: "SmtpConfigs",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "ceated_date",
                table: "SMSConfigs",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "ceated_date",
                table: "Documents",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "ceated_date",
                table: "Collaterals",
                newName: "created_date");

            migrationBuilder.CreateTable(
                name: "AccTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loancode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loandesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loaninterest = table.Column<double>(type: "float", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    loanrequire = table.Column<bool>(type: "bit", nullable: false),
                    sharecode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    savingscode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    interestcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shortname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processrate = table.Column<double>(type: "float", nullable: false),
                    chargecode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processind = table.Column<bool>(type: "bit", nullable: false),
                    normalloanind = table.Column<bool>(type: "bit", nullable: false),
                    memgroupacct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processamt = table.Column<double>(type: "float", nullable: false),
                    require_collateral = table.Column<bool>(type: "bit", nullable: false),
                    request_while_running = table.Column<bool>(type: "bit", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    repayOrder = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumbCompOrders",
                columns: table => new
                {
                    productname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false),
                    productcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumbCompOrders", x => new { x.productname, x.position });
                });

            migrationBuilder.CreateTable(
                name: "NumbComps",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    component = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumbComps", x => new { x.code, x.component });
                });

            migrationBuilder.CreateTable(
                name: "ProductNos",
                columns: table => new
                {
                    component = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    number = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNos", x => x.component);
                });

            migrationBuilder.CreateTable(
                name: "ProductTracks",
                columns: table => new
                {
                    productname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    productrange = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTracks", x => x.productname);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccTypes");

            migrationBuilder.DropTable(
                name: "LoanSetups");

            migrationBuilder.DropTable(
                name: "NumbCompOrders");

            migrationBuilder.DropTable(
                name: "NumbComps");

            migrationBuilder.DropTable(
                name: "ProductNos");

            migrationBuilder.DropTable(
                name: "ProductTracks");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "SmtpConfigs",
                newName: "ceated_date");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "SMSConfigs",
                newName: "ceated_date");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Documents",
                newName: "ceated_date");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Collaterals",
                newName: "ceated_date");
        }
    }
}
