using System.Linq;
using System.Security.Claims;

namespace JwtAuth.Core
{
    public static class ClaimExtensions
    {
        // TODO: this is keycloak specific?
        private const string ScopeKey = "scope";
        
        public static bool AuthorizeScopeClaim(this ClaimsPrincipal principal, string scope)
        {
            // TODO: can we allow for checking multiple possible scopes?
            var scopeClaim = principal.Claims.SingleOrDefault(x => x.Type == ScopeKey);
            if (scopeClaim == null)
            {
                return false;
            }

            return scopeClaim.Value.Contains(scope);
        }
    }
}