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

            modelBuilder.Entity("PallyWad.Domain.Collateral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ceated_date")
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

            modelBuilder.Entity("PallyWad.Domain.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ceated_date")
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

                    b.ToTable("Documents");
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

                    b.Property<DateTime>("ceated_date")
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

                    b.Property<DateTime>("ceated_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isSSL")
                        .HasColumnType("bit");

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
#pragma warning restore 612, 618
        }
    }
}
