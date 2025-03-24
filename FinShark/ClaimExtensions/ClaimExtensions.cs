using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace FinShark.ClaimExtensions
{
    // Seems like the information about the claims are created via the token service
    // This would allow us to reach into those claims that we created and pull the information?
    public static class ClaimExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
        }
    }
}
