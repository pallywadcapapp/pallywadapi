using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PallyWad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Infrastructure.Data
{
    public partial class AccountDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public AccountDbContext(DbContextOptions<AccountDbContext> options, IHttpContextAccessor httpContextAccessor = null) : base(options)
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

        public virtual DbSet<GL> GL { get; set; }
        public virtual DbSet<GLAccount> GLAccounts { get; set; }
        public virtual DbSet<GLAccountBase> GLAccountBases { get; set; }
        public virtual DbSet<GLAccountB> GLAccountBs { get; set; }
        public virtual DbSet<GLAccountC> GLAccountCs { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<AccountBase> AccountBases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GL>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<GLAccount>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<GLAccountBase>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<GLAccountB>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<GLAccountC>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<Journal>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<AccountBase>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<Payment>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
        }
    }
}
