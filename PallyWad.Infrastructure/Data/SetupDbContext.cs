﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using PallyWad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Infrastructure.Data
{
    public partial class SetupDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public SetupDbContext(DbContextOptions<SetupDbContext> options, IHttpContextAccessor httpContextAccessor = null) : base(options)
        {
            _httpContext = httpContextAccessor?.HttpContext;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(new DirectoryInfo(Environment.CurrentDirectory).FullName)//System.AppDomain.CurrentDomain.BaseDirectory)
                                                                                      //.AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("Default");
            Console.WriteLine("cnn is " + connstr);
            optionsBuilder.UseSqlServer(connstr);//.UseSnakeCaseNamingConvention();
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<SmtpConfig> SmtpConfigs { get; set; }
        public virtual DbSet<SMSConfig> SMSConfigs { get; set; }
        public virtual DbSet<AccType> AccTypes { get; set; }
        public virtual DbSet<Collateral> Collaterals { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<LoanSetup> LoanSetups { get; set; }
        public virtual DbSet<ProductTrack> ProductTracks { get; set; }

        public virtual DbSet<ProductNo> ProductNos { get; set; }
        public virtual DbSet<NumbComp> NumbComps { get; set; }
        public virtual DbSet<NumbCompOrder> NumbCompOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmtpConfig>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<SMSConfig>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<Collateral>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<Document>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<NumbComp>()
         .HasKey(m => new { m.code, m.component });

            modelBuilder.Entity<NumbCompOrder>()
         .HasKey(m => new { m.productname, m.position });
        }
    }
}
