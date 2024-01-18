using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Application
{
    public static class IPWhitelistMiddlewareExtensions
    {
        public static IApplicationBuilder UseIPWhitelist(this
        IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPWhitelistMiddleware>();
        }
    }
}
