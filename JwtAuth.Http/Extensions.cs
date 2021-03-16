using System;
using System.Net.Http;

namespace JwtAuth.Http
{
    public static class Extensions
    {
        public static string ExtractAccessToken(this HttpRequestMessage req)
        {
            var jwtInput = req.Headers.Authorization;
            var jwt = "";
            if (jwtInput.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                jwt = jwtInput.ToString().Substring("Bearer ".Length).Trim();
            }

            return jwt;
        }
    }
}