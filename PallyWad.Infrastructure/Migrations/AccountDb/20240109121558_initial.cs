using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AccountDb
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetGLAccountCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    glaccta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accttype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shortdesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fulldesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acctlevel = table.Column<int>(type: "int", nullable: false),
                    internalind = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetGLAccountCs", x => x.Id);
                });

            migrationBuilder.CreateTable(
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
                });

            migrationBuilder.CreateTable(
                name: "GLAccountBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    glaccta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shortdesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fulldesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accttype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acctlevel = table.Column<short>(type: "smallint", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAccountBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GLAccountBs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    glaccta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accttype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shortdesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fulldesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acctlevel = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAccountBs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GLAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    glaccta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glacctd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accttype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shortdesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fulldesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acctlevel = table.Column<int>(type: "int", nullable: false),
                    internalind = table.Column<bool>(type: "bit", nullable: false),
                    reportMap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportMapSub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    balanceSheetMap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    voucherNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    transDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    receivedFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cashcodeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ccenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    debit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    credit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    postedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approvalStatus = table.Column<bool>(type: "bit", nullable: false),
                    approvalForm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    posterEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approvedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approvedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bank = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exchangeRate = table.Column<double>(type: "float", nullable: true),
                    payState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    voucherNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    transDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    receivedFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cashcodeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ccenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    debit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    credit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    postedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approvalStatus = table.Column<bool>(type: "bit", nullable: false),
                    approvalForm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    posterEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approvedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approvedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bank = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetGLAccountCs");

            migrationBuilder.DropTable(
                name: "GL");

            migrationBuilder.DropTable(
                name: "GLAccountBases");

            migrationBuilder.DropTable(
                name: "GLAccountBs");

            migrationBuilder.DropTable(
                name: "GLAccounts");

            migrationBuilder.DropTable(
                name: "Journals");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
