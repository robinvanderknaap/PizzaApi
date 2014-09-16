using System.Net.Http;
using System.Threading.Tasks;

namespace API.Infrastructure.Handlers
{
    public class AddSomeContentHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            // Pass through request and await response
            var response = await base.SendAsync(request, cancellationToken);

            if (response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content = new StringContent(content + "Pizza!");
            }

            return response;
        }
    }
}
