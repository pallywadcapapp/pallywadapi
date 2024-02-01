using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using PallyWad.Domain;
using PallyWad.Infrastructure.EntityConfig;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Infrastructure.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppIdentityUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
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
            optionsBuilder.UseSqlServer(connstr);
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<UserProfile> UserProfile { get; set; }

        public virtual DbSet<MemberAccount> MemberAccounts { get; set; }
        //public virtual DbSet<UserVehicle> UserVehicle { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<AppIdentityUser>()
            //.HasOne(a => a.UserProfile)
            //.WithOne(a => a.AppIdentityUser)
            //.HasForeignKey<UserProfile>(c => c.memberid);

            //builder.Entity<AppIdentityUser>()
            //.HasMany(a => a.account)
            //.WithOne(a => a.member);


            builder.Entity<MemberAccount>().Property(u => u.Id)
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            builder.Entity<MemberAccount>()
                .HasOne(u => u.member)
                .WithMany(u => u.account)
                .HasForeignKey(u => u.AppIdentityUserId);


            builder.ApplyConfiguration(new UserConfiguration());
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
