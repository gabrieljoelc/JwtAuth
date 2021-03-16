namespace JwtAuth.Core
{
    public class JwtServerConfig
    {
        public string WellKnownEndpoint { get; set; }
        public string AllowedScope { get; set; }
    }
}