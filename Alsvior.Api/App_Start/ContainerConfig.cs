using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alsvior.DAL;
using Alsvior.Representations;
using Alsvior.Slack;
using Alsvior.Weather;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace Alsvior.Api
{
    public class ContainerConfig
    {
        public static AutofacWebApiDependencyResolver GetApiDependencyResolver(IContainer container)
        {
            return new AutofacWebApiDependencyResolver(container);
        }

        public static IContainer BuildContainer(HttpConfiguration config = null)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ApiDependencyModule>();
            builder.RegisterModule<DALDependencyModule>();
            builder.RegisterModule<SlackDependencyModule>();
            builder.RegisterModule<WeatherDependencyModule>();
            if (config != null)
            {
                builder.RegisterWebApiFilterProvider(config);
            }
            var container = builder.Build();
            return container;
        }

        public static IContainer BuildServiceLocatorContainer()
        {
            return BuildContainer();
        }
    }
}
