using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Api.Infrastructure.Handlers
{
    // Handles unhandled exceptions, return a proper error response to client
    // http://www.asp.net/web-api/overview/web-api-routing-and-actions/web-api-global-error-handling
    public class UnhandledExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            // An exception handler indicates that it has handled an exception by setting the Result property to an action result 
            context.Result = new UnhandledErrorActionResult(context.ExceptionContext.Request);
        }

        private class UnhandledErrorActionResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _httpRequestMessage;

            public UnhandledErrorActionResult(HttpRequestMessage httpRequestMessage)
            {
                _httpRequestMessage = httpRequestMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                // Create response in case of an unhandled exception, you could return a custom error response for example
                var response = _httpRequestMessage.CreateResponse(HttpStatusCode.InternalServerError, new { Error = "InternalServerError", SomeOtherData = "Extra cheese not available"});
                
                return Task.FromResult(response);
            }
        }
    }
}
