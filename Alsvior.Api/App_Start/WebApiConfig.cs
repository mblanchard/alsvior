
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

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

            //CORS
            config.EnableCors();


            // Web API routes
            


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());
            //Return JSON
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

        }
    }
    public  class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
        GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>
            (inherit: true);
        }
    }
}
