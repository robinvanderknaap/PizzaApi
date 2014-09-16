using System;
using System.Web;
using System.Web.Http;
using API;
using Api.Host.DependencyResolver;
using Api.Infrastructure.Services;
using StructureMap;

namespace Api.Host
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = BuildContainer();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(config => config.DependencyResolver = new StructureMapDependencyResolver(container));
        }

        public static IContainer BuildContainer()
        {
            var container = new Container(c =>
            {
                c.For<IPizzaOven>().Use<PizzaOven>();
                c.For<ILogger>().Use<Logger>();
                c.For<IAuthenticationService>().Use<AuthenticationService>();
            });

            return container;
        }
    }
}