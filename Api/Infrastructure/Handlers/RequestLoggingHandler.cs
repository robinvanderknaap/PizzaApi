using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Infrastructure.Handlers
{
    public class RequestLoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            // Create new api request log item
            var apiRequestLogItem = new ApiRequestLogItem();

            // Wrap logging of request in try..catch statement. Request should not fail due to errors in logging
            try
            {
                // Get http method
                apiRequestLogItem.HttpMethod = request.Method.Method;

                // Get request url
                apiRequestLogItem.RequestUrl = request.RequestUri.OriginalString;

                // Retrieve current principal (user)
                var currentPrincipal = request.GetRequestContext().Principal as ClaimsPrincipal;

                // If user is authenticated, add user info to log item
                if (currentPrincipal != null && currentPrincipal.Identity.IsAuthenticated)
                {
                    apiRequestLogItem.Username = currentPrincipal.Identity.Name;
                }

                // Try to retrieve ip address. We use dynamic here, because we don't want to have a reference to System.Web
                // http://stackoverflow.com/a/23656728/426840
                try
                {
                    apiRequestLogItem.RemoteIp =
                        ((dynamic)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
                catch { /* Swallow! */}

                // Get request headers
                var requestHeaders =
                    request.Headers.Select(
                        header => string.Format("{0}: {1}", header.Key, string.Join(",", header.Value)));

                // Get request payload
                var requestPayload = await request.Content.ReadAsStringAsync();

                // Rebuild request by combining headers and payload and add to logitem.
                apiRequestLogItem.Request = CreateRequestResponseMessage(requestHeaders, requestPayload);
            }
            catch { /* Swallow! */}

            // Pass through request and await response
            var response = await base.SendAsync(request, cancellationToken);

            // Wrap logging of response in try..catch statement. Request should not fail due to errors in logging
            try
            {
                // Get response headers
                var responseHeaders =
                    response.Headers.Select(
                        header => string.Format("{0}: {1}", header.Key, string.Join(",", header.Value)));

                // Get response payload
                var responsePayload = await response.Content.ReadAsStringAsync();

                // Rebuild response by combining headers and payload and add to logitem.
                apiRequestLogItem.Response = CreateRequestResponseMessage(responseHeaders, responsePayload);
            
                // STORE YOUR LOG ITEM HERE!!
            }
            catch { /* Swallow! */}

            // Pass through response
            return response;

        }

        private string CreateRequestResponseMessage(IEnumerable<string> headers, string content)
        {
            return string.Format("{0}\r\n{1}\r\n\r\n", string.Join("\r\n", headers), content);
        }

        private class ApiRequestLogItem
        {
            public ApiRequestLogItem()
            {
                Created = DateTime.UtcNow;
            }
            public string Username { get; set; }
            public string RequestUrl { get; set; }
            public string RemoteIp { get; set; }
            public string HttpMethod { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
            public DateTime Created { get; private set; }
        }
    }
}
