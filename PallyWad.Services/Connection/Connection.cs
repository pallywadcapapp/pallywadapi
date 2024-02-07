using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PallyWad.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Connection
{
    public static class Connection
    {
        public static void Setup(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                Development(builder);
            }
            else if (builder.Environment.IsStaging())
            {
                Staging(builder);
            }
            else if (builder.Environment.IsProduction())
            {
                Production(builder);
            }
        }

        private static void Production(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Development")));
            //.LogTo(new IDbContextLogger().log, (id, _) => id == RelationalEventId.CommandExecuting);
        }

        private static void Staging(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Staging")));
        }

        private static void Development(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Production")));
        }
    }
}
