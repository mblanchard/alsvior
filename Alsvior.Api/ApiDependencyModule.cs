﻿using Alsvior.Api.Controllers;
using Alsvior.Api.Controllers.Geospatial;
using Alsvior.Api.Controllers.GeospatialDatasets;
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
            //Datasets
            builder.RegisterType<WeatherNodeController>().InstancePerRequest();
            builder.RegisterType<InverterNodeController>().InstancePerRequest();
            builder.RegisterType<WeatherHourlyController>().InstancePerRequest();
            builder.RegisterType<WeatherDailyController>().InstancePerRequest();
            builder.RegisterType<WeatherDailyController>().InstancePerRequest();

            //Metadata
            builder.RegisterType<DatasetMetadataController>().InstancePerRequest();
            builder.RegisterType<NotificationController>().InstancePerRequest();
            
            base.Load(builder);
        }
    }
}