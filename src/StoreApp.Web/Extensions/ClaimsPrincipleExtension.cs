using System.Security.Claims;

namespace StoreApp.Web.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string? GetPhoneNumber(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("PhoneNumber")?.Value;
        }
    }
}
