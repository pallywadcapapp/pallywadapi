﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PallyWad.Infrastructure.Data;

#nullable disable

namespace PallyWad.Infrastructure.Migrations.SetupDb
{
    [DbContext(typeof(SetupDbContext))]
    partial class SetupDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PallyWad.Domain.AccType", b =>
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

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("AccTypes");
                });

            modelBuilder.Entity("PallyWad.Domain.Charges", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("accountno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("chargecode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("chargedesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("shortname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Charges");
                });

            modelBuilder.Entity("PallyWad.Domain.Collateral", b =>
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

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Collaterals");
                });

            modelBuilder.Entity("PallyWad.Domain.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("accountno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bank")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("PallyWad.Domain.Document", b =>
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

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("PallyWad.Domain.Interest", b =>
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

                    b.Property<string>("interestcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("interestdesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("payableacctno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("shortname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Interests");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanCollateral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LoanSetupId")
                        .HasColumnType("int");

                    b.Property<string>("collateralId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LoanSetupId");

                    b.ToTable("LoanCollaterals");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LoanSetupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("documentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LoanSetupId");

                    b.ToTable("LoanDocuments");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanSetup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("accountno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("age")
                        .HasColumnType("int");

                    b.Property<string>("category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("chargecode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("collateralPercentage")
                        .HasColumnType("float");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("duration")
                        .HasColumnType("int");

                    b.Property<string>("interestcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loanInterestType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loancode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loandesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("loaninterest")
                        .HasColumnType("float");

                    b.Property<bool>("loanrequire")
                        .HasColumnType("bit");

                    b.Property<string>("memgroupacct")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("normalloanind")
                        .HasColumnType("bit");

                    b.Property<double>("processamt")
                        .HasColumnType("float");

                    b.Property<bool>("processind")
                        .HasColumnType("bit");

                    b.Property<double>("processrate")
                        .HasColumnType("float");

                    b.Property<int>("repayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("request_while_running")
                        .HasColumnType("bit");

                    b.Property<bool>("require_collateral")
                        .HasColumnType("bit");

                    b.Property<string>("shortname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("LoanSetups");
                });

            modelBuilder.Entity("PallyWad.Domain.NumbComp", b =>
                {
                    b.Property<string>("code")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<string>("component")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("code", "component");

                    b.ToTable("NumbComps");
                });

            modelBuilder.Entity("PallyWad.Domain.NumbCompOrder", b =>
                {
                    b.Property<string>("productname")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("position")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("productcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("productname", "position");

                    b.ToTable("NumbCompOrders");
                });

            modelBuilder.Entity("PallyWad.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isDefault")
                        .HasColumnType("bit");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PallyWad.Domain.ProductNo", b =>
                {
                    b.Property<string>("component")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("number")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("component");

                    b.ToTable("ProductNos");
                });

            modelBuilder.Entity("PallyWad.Domain.ProductTrack", b =>
                {
                    b.Property<string>("productname")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("productrange")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.HasKey("productname");

                    b.ToTable("ProductTracks");
                });

            modelBuilder.Entity("PallyWad.Domain.SMSConfig", b =>
                {
                    b.Property<string>("configname")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("configname");

                    b.ToTable("SMSConfigs");
                });

            modelBuilder.Entity("PallyWad.Domain.SmtpConfig", b =>
                {
                    b.Property<string>("configname")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isSSL")
                        .HasColumnType("bit");

                    b.Property<string>("mailfrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("port")
                        .HasColumnType("int");

                    b.Property<string>("smtp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("configname");

                    b.ToTable("SmtpConfigs");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanCollateral", b =>
                {
                    b.HasOne("PallyWad.Domain.LoanSetup", "loanSetup")
                        .WithMany("LoanCollaterals")
                        .HasForeignKey("LoanSetupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("loanSetup");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanDocument", b =>
                {
                    b.HasOne("PallyWad.Domain.LoanSetup", "loanSetup")
                        .WithMany("LoanDocuments")
                        .HasForeignKey("LoanSetupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("loanSetup");
                });

            modelBuilder.Entity("PallyWad.Domain.LoanSetup", b =>
                {
                    b.Navigation("LoanCollaterals");

                    b.Navigation("LoanDocuments");
                });
#pragma warning restore 612, 618
        }
    }
}
