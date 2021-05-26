using System.Security.Claims;

namespace DatingApp.Api.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user){
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            return username;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}