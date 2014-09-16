using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using Api.Infrastructure.Services;

namespace Api.Infrastructure.Handlers
{
    // Logs unhandled exceptions
    // http://www.asp.net/web-api/overview/web-api-routing-and-actions/web-api-global-error-handling
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var logger = context.Request.GetDependencyScope().GetService(typeof(ILogger)) as ILogger;

            if (logger != null && context.Exception != null)
            {
                logger.Log(context.Exception.Message);
            }
        }
    }
}
