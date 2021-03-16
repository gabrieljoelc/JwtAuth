using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtAuth.Core
{
    public interface IJwtValidator
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(string accessToken);
    }
}