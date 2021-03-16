using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JwtAuth.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JwtAuth.Http
{
    internal class NetHttpAuthorizer : INetHttpAuthorizer
    {
        public NetHttpAuthorizer(IJwtValidator validator, IOptions<JwtServerConfig> config,
            ILogger<NetHttpAuthorizer> logger)
        {
            Validator = validator;
            Logger = logger;
            // TODO: allow for more than one scope?
            AllowedScope = config.Value.AllowedScope;
        }

        private string AllowedScope { get; }

        private IJwtValidator Validator { get; }
        private ILogger<NetHttpAuthorizer> Logger { get; }

        public async Task<HttpStatusCode> Authorize(HttpRequestMessage req)
        {
            var jwtInput = req.ExtractAccessToken();
            if (jwtInput == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            var principal = await Validator.ValidateTokenAsync(jwtInput);
            if (principal == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            return principal.AuthorizeScopeClaim(AllowedScope) ? HttpStatusCode.Accepted : HttpStatusCode.Unauthorized;
        }
    }
}