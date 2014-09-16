using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Api.Infrastructure.Filters;
using API.Infrastructure.Handlers;
using Api.Infrastructure.Handlers;
using Newtonsoft.Json.Serialization;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable http attribute routes
            config.MapHttpAttributeRoutes();

            // Setup json formatter to use camelcasing even when responses are pascal cased
            // http://msmvps.com/blogs/theproblemsolver/archive/2014/03/26/webapi-pascalcase-and-camelcase.aspx
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Remove xml formatter, we only support json.
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Add global filter
            config.Filters.Add(new DebugFilter());

            // Add handlers
            config.MessageHandlers.Add(new AuthenticationHandler()); // Always add first
            config.MessageHandlers.Add(new RequestLoggingHandler());
            config.MessageHandlers.Add(new HttpExceptionHandler());
            config.MessageHandlers.Add(new FirstTestHandler());
            config.MessageHandlers.Add(new SecondTestHandler());
            //config.MessageHandlers.Add(new AddSomeContentHandler()); // Uncomment this if you want 'Pizza!' added to your response

            // Setup handling and logging of unhandled exceptions
            // http://www.asp.net/web-api/overview/web-api-routing-and-actions/web-api-global-error-handling
            config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
        }
    }
}
