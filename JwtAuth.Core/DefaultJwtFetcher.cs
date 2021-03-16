using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JwtAuth.Core
{
    public interface IJwtFetcher
    {
        Task<string> GetAccessToken();
    }

    /// <summary>
    /// Fetches a valid JWT if the current isn't set or expired. Stores current token statically in the class.
    /// </summary>
    internal class DefaultJwtFetcher : IJwtFetcher
    {
        public DefaultJwtFetcher(JwtClientConfig config)
        {
            Config = config;
            FormMap =
                JwtFetcherConfigExtensions.ConfigProperties.Value.ToDictionary(info => info.Key,
                    info => info.Value.GetValue(Config, null).ToString());
        }

        // TODO: move to DI management?
        private static AccessTokenResponse _currentToken;

        private static HttpClient Client { get; } = new HttpClient();
        private JwtClientConfig Config { get; }

        private Dictionary<string, string> FormMap { get; }

        public async Task<string> GetAccessToken()
        {
            if (_currentToken?.Expires.CompareTo(DateTime.UtcNow) > 0)
                return _currentToken.AccessToken; //Current token still valid

            var content = new FormUrlEncodedContent(FormMap);
                
            var res = await Client.PostAsync(Config.TokenUrl, content);
            if (res.IsSuccessStatusCode == false)
            {
                return string.Empty;
            }

            var body = await res.Content.ReadAsStringAsync();
            _currentToken = JsonConvert.DeserializeObject<AccessTokenResponse>(body);
            return _currentToken.AccessToken;
        }
    }
}