using System;
using Newtonsoft.Json;

namespace JwtAuth.Core
{
    internal class AccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken {get;set;}

        [JsonProperty("expires_in")]
        public int ExpiresIn {get;set;}

        public DateTime Created = DateTime.UtcNow;
        public DateTime Expires => Created.AddSeconds(ExpiresIn);
    }
}