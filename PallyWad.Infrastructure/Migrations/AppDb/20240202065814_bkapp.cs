using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class bkapp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transOwner",
                table: "AppUploadedFiles",
                type: "nvarchar(max)",
                nullable: true);

            /*migrationBuilder.CreateTable(
                name: "GL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    refnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    glaccta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    batchno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chequeno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    debitamt = table.Column<double>(type: "float", nullable: false),
                    creditamt = table.Column<double>(type: "float", nullable: false),
                    balance = table.Column<double>(type: "float", nullable: false),
                    userid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acc_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    ref_trans = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GL", x => x.Id);
                });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropTable(
                name: "GL");*/

            migrationBuilder.DropColumn(
                name: "transOwner",
                table: "AppUploadedFiles");
        }
    }
}
