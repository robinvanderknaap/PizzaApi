using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Api.Infrastructure.Services;
using Api.Infrastructure.Util;

namespace Api.Infrastructure.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private const string Scheme = AuthenticationTypes.Basic;

        private IAuthenticationService _authenticationService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Message handler is created on application start, not per request, so we need to resolve the authentication service using the dependencyscope (this is in request scope)
            _authenticationService = request.GetDependencyScope().GetService(typeof(IAuthenticationService)) as IAuthenticationService;
            
            if (_authenticationService == null)
            {
                throw new ApplicationException("Authentication service cannot be resolved");
            }

            // Try get authentication header
            var authorisationHeader = request.Headers.Authorization;

            // If authentication header is available and is set to basic authentication
            if (authorisationHeader != null && Scheme.Equals(authorisationHeader.Scheme))
            {
                string username;
                string password;
                
                try
                {
                    username = authorisationHeader.Username();
                    password = authorisationHeader.Password();
                }
                catch
                {
                    // If authorization header cannot be properly read, give back sensible error
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Authorization header is not properly formatted.");
                }

                // Authenticate request
                var authenticateResponse = _authenticationService.IsAuthenticated(username, password);

                // Set user if authentication is successfull
                if (authenticateResponse)
                {
                    var claimsIdentity = new ClaimsIdentity(AuthenticationTypes.Password);
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, username));

                    // Do not use thread.principal or httpcontext to set user. This will create dependency on system.web, which stands in the way of self hosting
                    // http://brockallen.com/2013/10/27/host-authentication-and-web-api-with-owin-and-active-vs-passive-authentication-middleware/
                    request.GetRequestContext().Principal = new ClaimsPrincipal(claimsIdentity);
                }
            }

            // Process request
            var response = await base.SendAsync(request, cancellationToken);

            // Add header to indicate basic authentication is needed when request is unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Scheme));
                response.Content = new JsonContent(new { Error = "Unauthorized" });
            }

            return response;
        }
    }
}

