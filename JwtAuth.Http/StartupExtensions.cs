using Microsoft.Extensions.DependencyInjection;

namespace JwtAuth.Http
{
    public static class StartupExtensions
    {
        public static IServiceCollection UseJwtHttpAuthorizer(this IServiceCollection collection)
        {
            collection.AddTransient<INetHttpAuthorizer, NetHttpAuthorizer>();
            return collection;
        }
    }
}