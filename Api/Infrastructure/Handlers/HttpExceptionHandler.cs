using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Api.Infrastructure.Util;

namespace Api.Infrastructure.Handlers
{
    public class HttpExceptionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Process request
            var response = await base.SendAsync(request, cancellationToken);

            // Set default error object when resource is not found
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                response.Content = new JsonContent(new { Message = "The requested resource is not found" });
            }

            // Set default object when method is not allowed
            if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                response.Content = new JsonContent(new { Message = "Method not allowed" });
            }

            return response;
        }
    }
}

