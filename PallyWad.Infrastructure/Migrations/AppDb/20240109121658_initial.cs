using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUploadedFiles",
                columns: table => new
                {
                    filename = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fileurl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uploaderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    extractedStatus = table.Column<bool>(type: "bit", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUploadedFiles", x => x.filename);
                });

            migrationBuilder.CreateTable(
                name: "BankDeposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    depositId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    otherdetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loanDeductAmount = table.Column<double>(type: "float", nullable: true),
                    loanRefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    memberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    requestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approvalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    approvedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDeposits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanRepayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loanrefnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    repayrefnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    loancode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loanamount = table.Column<double>(type: "float", nullable: false),
                    repayamount = table.Column<double>(type: "float", nullable: false),
                    interestamt = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transmonth = table.Column<int>(type: "int", nullable: false),
                    transyear = table.Column<int>(type: "int", nullable: false),
                    updated = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanRepayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loancode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    memberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bvn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankaccountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    savBal = table.Column<double>(type: "float", nullable: true),
                    loanBal = table.Column<double>(type: "float", nullable: true),
                    netBal = table.Column<double>(type: "float", nullable: true),
                    othercoorp = table.Column<double>(type: "float", nullable: false),
                    monthtotalrepay = table.Column<double>(type: "float", nullable: true),
                    monthlyrepay = table.Column<double>(type: "float", nullable: true),
                    monthlyendsalary = table.Column<double>(type: "float", nullable: true),
                    monthlnetsalary = table.Column<double>(type: "float", nullable: true),
                    guarantorId1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    guarantorId2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    guarantorId3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    requestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approvalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    approvedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duration = table.Column<int>(type: "int", nullable: true),
                    loaninterest = table.Column<double>(type: "float", nullable: true),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processingFee = table.Column<double>(type: "float", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanTrans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loanrefnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    loancode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    repaystartdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    loanamount = table.Column<double>(type: "float", nullable: false),
                    totrepayable = table.Column<double>(type: "float", nullable: false),
                    repayamount = table.Column<double>(type: "float", nullable: false),
                    interestamt = table.Column<double>(type: "float", nullable: false),
                    repay = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payableacctno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    grefnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gapproved = table.Column<bool>(type: "bit", nullable: false),
                    loaninterest = table.Column<double>(type: "float", nullable: false),
                    processrate = table.Column<double>(type: "float", nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    processamt = table.Column<double>(type: "float", nullable: false),
                    updated = table.Column<bool>(type: "bit", nullable: false),
                    stopdoubleinterest = table.Column<bool>(type: "bit", nullable: false),
                    glrefnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glbankaccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    repayOrder = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTrans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCollaterals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    colleteralId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    estimatedValue = table.Column<double>(type: "float", nullable: false),
                    approvedValue = table.Column<double>(type: "float", nullable: false),
                    loanRefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verificationStatus = table.Column<bool>(type: "bit", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollaterals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    documentRefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUploadedFiles");

            migrationBuilder.DropTable(
                name: "BankDeposits");

            migrationBuilder.DropTable(
                name: "LoanRepayments");

            migrationBuilder.DropTable(
                name: "LoanRequests");

            migrationBuilder.DropTable(
                name: "LoanTrans");

            migrationBuilder.DropTable(
                name: "UserCollaterals");

            migrationBuilder.DropTable(
                name: "UserDocuments");
        }
    }
}
