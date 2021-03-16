#if NET461
// nothing right now
#else 
// dotnet standard 2.0

using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection UseDefaultJwtValidator(this IServiceCollection collection)
        {
            collection.AddTransient<ISecurityTokenValidator, JwtSecurityTokenHandler>()
                .AddTransient<IJwtValidator, DefaultJwtValidator>()
                .AddOptions<JwtServerConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(JwtServerConfig)).Bind(settings);
                });
            return collection;
        }
    }
}

#endif