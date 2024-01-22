using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AccountDb
{
    /// <inheritdoc />
    public partial class ggl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GetGLAccountCs",
                table: "GetGLAccountCs");

            migrationBuilder.RenameTable(
                name: "GetGLAccountCs",
                newName: "GLAccountCs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAccountCs",
                table: "GLAccountCs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAccountCs",
                table: "GLAccountCs");

            migrationBuilder.RenameTable(
                name: "GLAccountCs",
                newName: "GetGLAccountCs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetGLAccountCs",
                table: "GetGLAccountCs",
                column: "Id");
        }
    }
}
