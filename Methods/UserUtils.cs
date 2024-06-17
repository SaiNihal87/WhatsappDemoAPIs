using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WhatsappDemoAPIs.Methods;
public static class UserUtils
{
    public static long GetUserId(HttpContext httpContext)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        var userClaims = identity?.Claims;
        if (userClaims != null)
        {
            var userIdClaim = userClaims.FirstOrDefault(o => o.Type == "id")?.Value;
            if (long.TryParse(userIdClaim, out long userId))
            {
                return userId;
            }
        }
        
        return -1; 
    }
}

