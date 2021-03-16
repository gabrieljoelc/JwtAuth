using JwtAuth.Core;
using JwtAuth.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(JwtAuth.FunctionDemo.Startup))]

namespace JwtAuth.FunctionDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                // would be used by server
                .UseDefaultJwtValidator()
                .UseJwtHttpAuthorizer()
                // for the sample, we reuse the same client config but for .NET Framework apps
                // it will be something else at the top-level
                .AddOptions<JwtClientConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(JwtClientConfig)).Bind(settings);
                });
        }
    }
}