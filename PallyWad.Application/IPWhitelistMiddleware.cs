using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PallyWad.Application
{
    public class IPWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPWhitelistOptions _iPWhitelistOptions;
        private readonly ILogger<IPWhitelistMiddleware> _logger;
        public IPWhitelistMiddleware(RequestDelegate next,
        ILogger<IPWhitelistMiddleware> logger,
            IOptions<IPWhitelistOptions> applicationOptionsAccessor)
        {
            _iPWhitelistOptions = applicationOptionsAccessor.Value;
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethod.Get.Method)
            {
                var ipAddress = context.Connection.RemoteIpAddress;
                List<string> whiteListIPList =
                _iPWhitelistOptions.Whitelist;
                var isIPWhitelisted = whiteListIPList
                .Where(ip => IPAddress.Parse(ip)
                .Equals(ipAddress))
                .Any();
                if (!isIPWhitelisted)
                {
                    _logger.LogWarning(
                    "Request from Remote IP address: {RemoteIp} is forbidden.", ipAddress);
                    context.Response.StatusCode =
                    (int)HttpStatusCode.Forbidden;
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}
