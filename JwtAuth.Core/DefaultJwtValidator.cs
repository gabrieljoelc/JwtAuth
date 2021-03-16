#if NET461
// nothing right now
#else 
// dotnet standard 2.0

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Core
{
    internal class DefaultJwtValidator : IJwtValidator
    {
        private ILogger<DefaultJwtValidator> Log { get; }
        private string WellKnownEndpoint { get; }
        private ISecurityTokenValidator TokenValidator { get; }

        public DefaultJwtValidator(IOptions<JwtServerConfig> options, ILogger<DefaultJwtValidator> log,
            ISecurityTokenValidator tokenValidator)
        {
            WellKnownEndpoint = options.Value.WellKnownEndpoint;
            Log = log;
            TokenValidator = tokenValidator;
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string accessToken)
        {
            var wellknownEndpoints = await GetOidcWellknownConfiguration();

            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKeys = wellknownEndpoints.SigningKeys,
                ValidIssuer = wellknownEndpoints.Issuer
            };

            return TokenValidator.ValidateToken(accessToken, validationParameters, out _);
        }

        private async Task<OpenIdConnectConfiguration> GetOidcWellknownConfiguration()
        {
            Log.LogDebug($"Get OIDC well known endpoints {WellKnownEndpoint}");
            var manager = new ConfigurationManager<OpenIdConnectConfiguration>(
                 WellKnownEndpoint, new OpenIdConnectConfigurationRetriever());

            return await manager.GetConfigurationAsync();
        }
    }
}
#endif