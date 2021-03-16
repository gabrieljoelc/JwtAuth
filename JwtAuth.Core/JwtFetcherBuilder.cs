using System;

namespace JwtAuth.Core
{
    public class JwtFetcherBuilder
    {
        public JwtFetcherBuilder()
        {
            Config = new JwtClientConfig();
        }

        private JwtClientConfig Config { get; }
        
        public JwtFetcherBuilder Initialize(Action<JwtClientConfig> action)
        {
            action(Config);
            return this;
        }

        public IJwtFetcher Build()
        {
            // TODO: build up with dotnet framework and core compatible DI?
            return new DefaultJwtFetcher(Config);
        }
    }
}