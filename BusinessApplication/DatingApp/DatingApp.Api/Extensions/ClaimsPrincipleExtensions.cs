using System.Security.Claims;

namespace DatingApp.Api.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user){
            var username = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return username;
        } 
    }
}