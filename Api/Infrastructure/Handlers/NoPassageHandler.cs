using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Infrastructure.Handlers
{
    public class NoPassageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Send message back to whereever it came from.
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
