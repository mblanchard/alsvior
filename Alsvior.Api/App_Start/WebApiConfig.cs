
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Alsvior.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Container Config
            var container = ContainerConfig.BuildContainer(config);
            var webApiResolver = ContainerConfig.GetApiDependencyResolver(container);
            config.DependencyResolver = webApiResolver;


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
