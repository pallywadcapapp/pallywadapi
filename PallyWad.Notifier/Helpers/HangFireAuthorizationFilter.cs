using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace PallyWad.Notifier.Helpers
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool _Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;

            string header = httpContext.Request.Headers.Authorization;
            if (string.IsNullOrWhiteSpace(header))
            {
                this.UnauthorizedResponse(httpContext);
                return false;
            }

            AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header);
            if (!"Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                this.UnauthorizedResponse(httpContext);
                return false;
            }

            // The problem is how to parse it here
            string[] loginInfo = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter)).Split(':');
            if (loginInfo.Length < 2)
            {
                this.UnauthorizedResponse(httpContext);
                return false;
            }

            string loginId = loginInfo[0], password = loginInfo[1];
            if (string.IsNullOrWhiteSpace(loginId) || string.IsNullOrWhiteSpace(password))
            {
                this.UnauthorizedResponse(httpContext);
                return false;
            }

            // This will be verified based on database users
            if (loginId == "wee" && password == "wee")
                return true;

            this.UnauthorizedResponse(httpContext);
            return false;
        }

        private void UnauthorizedResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            httpContext.Response.WriteAsync("Unauthorized.");
        }
    }
}
