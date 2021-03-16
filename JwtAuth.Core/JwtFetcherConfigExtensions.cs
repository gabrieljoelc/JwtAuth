using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace JwtAuth.Core
{
    public static class JwtFetcherConfigExtensions
    {
        internal static readonly Lazy<IDictionary<string, PropertyInfo>> ConfigProperties =
            new Lazy<IDictionary<string, PropertyInfo>>(
                () =>
                    typeof(JwtClientConfig)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(
                            info => CustomAttributeExtensions.GetCustomAttribute<JsonPropertyAttribute>((MemberInfo) info)?.PropertyName ?? info.Name,
                            info => info));

        public static void UseClientCredentialFlow(this JwtClientConfig config, string tokenUrl, string clientId,
            string grantType, string clientSecret, string scope)
        {
            config.TokenUrl = tokenUrl;
            config.ClientId = clientId;
            config.GrantType = grantType;
            config.ClientSecret = clientSecret;
            config.Scope = scope;
        }
    }
}