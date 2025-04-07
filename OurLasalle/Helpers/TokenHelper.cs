using Microsoft.AspNetCore.Http;

namespace OurLasalle.Helpers
{
    public static class TokenHelper
    {
        public static string? GetAccessToken(HttpContext httpContext)
        {
            return httpContext.Request.Cookies["AccessToken"];
        }

        public static string? GetRefreshToken(HttpContext httpContext)
        {
            return httpContext.Request.Cookies["RefreshToken"];
        }
    }
}
