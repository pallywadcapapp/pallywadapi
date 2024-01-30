﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PallyWad.Infrastructure.Data;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240130014540_declr")]
    partial class declr
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PallyWad.Domain.AppUploadedFiles", b =>
                {
                    b.Property<string>("filename")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("extractedStatus")
                        .HasColumnType("bit");

                    b.Property<string>("fileurl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("uploaderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("year")
                        .HasColumnType("int");

                    b.HasKey("filename");

                    b.ToTable("AppUploadedFiles");
                });

            modelBuilder.Entity("PallyWad.Domain.BankDeposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<DateTime?>("approvalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("approvedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("channel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("depositId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("loanDeductAmount")
                        .HasColumnType("float");

                    b.Property<string>("loanRefId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("memberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("otherdetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("postedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("processState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("requestDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BankDeposits");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanRepayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("interestamt")
                        .HasColumnType("float");

                    b.Property<double>("loanamount")
                        .HasColumnType("float");

                    b.Property<string>("loancode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loanrefnumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("memberid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("repayamount")
                        .HasColumnType("float");

                    b.Property<string>("repayrefnumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("transdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("transmonth")
                        .HasColumnType("int");

                    b.Property<int>("transyear")
                        .HasColumnType("int");

                    b.Property<int>("updated")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("LoanRepayments");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<DateTime?>("approvalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("approvedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bankaccountno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bankname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bvn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("collateralId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("duration")
                        .HasColumnType("int");

                    b.Property<double?>("loanBal")
                        .HasColumnType("float");

                    b.Property<string>("loanId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loancode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("loaninterest")
                        .HasColumnType("float");

                    b.Property<string>("memberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("monthlnetsalary")
                        .HasColumnType("float");

                    b.Property<double?>("monthlyendsalary")
                        .HasColumnType("float");

                    b.Property<double?>("monthlyrepay")
                        .HasColumnType("float");

                    b.Property<double?>("monthtotalrepay")
                        .HasColumnType("float");

                    b.Property<double?>("netBal")
                        .HasColumnType("float");

                    b.Property<string>("postedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("processState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("processingFee")
                        .HasColumnType("float");

                    b.Property<string>("reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("requestDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("savBal")
                        .HasColumnType("float");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("LoanRequests");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanTrans", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("accountno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("duration")
                        .HasColumnType("int");

                    b.Property<bool>("gapproved")
                        .HasColumnType("bit");

                    b.Property<string>("glbankaccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("glrefnumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("grefnumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("interestamt")
                        .HasColumnType("float");

                    b.Property<double>("loanamount")
                        .HasColumnType("float");

                    b.Property<string>("loancode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("loaninterest")
                        .HasColumnType("float");

                    b.Property<string>("loanrefnumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("memberid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("payableacctno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("processamt")
                        .HasColumnType("float");

                    b.Property<double>("processrate")
                        .HasColumnType("float");

                    b.Property<int>("repay")
                        .HasColumnType("int");

                    b.Property<int>("repayOrder")
                        .HasColumnType("int");

                    b.Property<double>("repayamount")
                        .HasColumnType("float");

                    b.Property<DateTime>("repaystartdate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("stopdoubleinterest")
                        .HasColumnType("bit");

                    b.Property<double>("totrepayable")
                        .HasColumnType("float");

                    b.Property<DateTime>("transdate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("updated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("LoanTrans");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanUserCollateral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("loanRequestId")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("userCollateralId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("loanRequestId");

                    b.ToTable("LoanUserCollaterals");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanUserDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("loanRequestId")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("userDocumentlId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("loanRequestId");

                    b.ToTable("LoanUserDocuments");
                });

            modelBuilder.Entity("PallyWad.Domain.UserCollateral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("approvedValue")
                        .HasColumnType("float");

                    b.Property<string>("colleteralId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<double>("estimatedValue")
                        .HasColumnType("float");

                    b.Property<string>("loanRefId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("otherdetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("verificationStatus")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("UserCollaterals");
                });

            modelBuilder.Entity("PallyWad.Domain.UserDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("doctype")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("documentNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("documentRefId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("expiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserDocuments");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanUserCollateral", b =>
                {
                    b.HasOne("PallyWad.Domain.LoanRequest", "loanRequest")
                        .WithMany("loanUserCollaterals")
                        .HasForeignKey("loanRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("loanRequest");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanUserDocument", b =>
                {
                    b.HasOne("PallyWad.Domain.LoanRequest", "loanRequest")
                        .WithMany("loanUserDocuments")
                        .HasForeignKey("loanRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("loanRequest");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanRequest", b =>
                {
                    b.Navigation("loanUserCollaterals");

                    b.Navigation("loanUserDocuments");
                });
#pragma warning restore 612, 618
        }
    }
}
