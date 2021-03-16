using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JwtAuth.Http
{
    public interface INetHttpAuthorizer
    {
        Task<HttpStatusCode> Authorize(HttpRequestMessage req);
    }
}