using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Infrastructure.Handlers
{
    public class SecondTestHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Pass through request and await response
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
