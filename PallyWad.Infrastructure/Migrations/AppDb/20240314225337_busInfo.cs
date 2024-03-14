using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class busInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    info_medium = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isRegistered = table.Column<bool>(type: "bit", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    landmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cacno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInformation", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessInformation");
        }
    }
}
