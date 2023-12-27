using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        //public virtual DbSet<UserProfile> UserProfile { get; set; }
        //public virtual DbSet<UserVehicle> UserVehicle { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
