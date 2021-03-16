using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JwtAuth.Core;
using JwtAuth.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JwtAuth.FunctionDemo
{
    public class ClientServer
    {
        public ClientServer(IOptions<JwtClientConfig> options, INetHttpAuthorizer authorizer)
        {
            Authorizer = authorizer;
            JwtFetcher = new JwtFetcherBuilder()
                .Initialize(fetcherConfig =>
                {
                    var config = options.Value;
                    fetcherConfig.UseClientCredentialFlow(config.TokenUrl, config.ClientId, config.GrantType,
                        config.ClientSecret, config.Scope);
                })
                .Build();
        }
        
        private INetHttpAuthorizer Authorizer { get; }

        [FunctionName("Server")]
        public async Task<IActionResult> RunServer(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger Server processed a request.");
            var status = await Authorizer.Authorize(req);
                
            return new JsonHttpStatusResult(new {result = Enum.GetName(typeof(HttpStatusCode), status)},
                status);
        }

        private IJwtFetcher JwtFetcher { get; }
        private static HttpClient Client { get; } = new HttpClient();
        
        [FunctionName("Client")]
        public async Task<IActionResult> RunClient(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger Client processed a request.");
                
            var accessToken = await JwtFetcher.GetAccessToken();
            
            var request = new HttpRequestMessage();
            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {accessToken}");
            request.RequestUri = new Uri("http://localhost:7071/api/Server");
            var res = await Client.SendAsync(request);
            var body = await res.Content.ReadAsStringAsync();

            // obviously don't return accessToken to client in production
            return new JsonResult(new {accessToken, serverResponseBody = JsonConvert.DeserializeObject(body)});
        }

        private class JsonHttpStatusResult : JsonResult
        {
            public JsonHttpStatusResult(object data, HttpStatusCode httpStatus) : base(data)
            {
                HttpStatus = httpStatus;
            }

            private HttpStatusCode HttpStatus { get; }

            public override Task ExecuteResultAsync(ActionContext context)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatus;
                return base.ExecuteResultAsync(context);
            }

            public override void ExecuteResult(ActionContext context)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatus;
                base.ExecuteResult(context);
            }
        }
    }
}