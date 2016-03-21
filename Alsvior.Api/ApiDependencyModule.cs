using Alsvior.Api.Controllers;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alsvior.Api
{
    public class ApiDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WeatherController>().InstancePerRequest();
            base.Load(builder);
        }
    }
}