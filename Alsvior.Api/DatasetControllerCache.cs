using Alsvior.Api.Controllers;
using Alsvior.Representations.Models;
using Alsvior.Representations.Models.DatasetEndpointDiscovery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsvior.Api
{
    public static class DatasetControllerCache
    {
        private static List<GeospatialDatasetEndpoint> endpoints = new List<GeospatialDatasetEndpoint>();

        public static void Init()
        {

            var datasetsByModel = GetGeospatialEndpointsByModel();
            var timeSeriesDatasetsByModel = AddTimeSeriesEndpointsToGeospatialEndpointsByModel(datasetsByModel);
            endpoints = timeSeriesDatasetsByModel?.Values?.ToList();         
        }

        private static Dictionary<Type, GeospatialDatasetEndpoint> GetGeospatialEndpointsByModel()
        {
            var datasetsByModel = new Dictionary<Type, GeospatialDatasetEndpoint>();
            
            //Find all controllers that extend GeospatialDatasetController with routePrefix attribute defined
            var geospatialControllers = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes())
               .Where(p => p.IsClass && p.BaseType != null && p.BaseType.IsGenericType && p.BaseType.GetGenericTypeDefinition() == (typeof(GeospatialDatasetController<>)) && p.CustomAttributes.Any(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute" && att.ConstructorArguments.FirstOrDefault().ArgumentType == typeof(string))).ToList();
            if (geospatialControllers.Any())
            {
                //Find the underlying ILocatable type associated to each Geospatial controller, allow for one controller per concretion of ILocatable
                foreach (var ctlr in geospatialControllers)
                {
                    var geoType = ctlr.BaseType.GenericTypeArguments.FirstOrDefault(x => x.GetInterface("ILocatable") != null);
                    if (geoType != null && !datasetsByModel.ContainsKey(geoType))
                    {
                        datasetsByModel.Add(geoType, new GeospatialDatasetEndpoint(geoType.Name, ctlr.CustomAttributes.FirstOrDefault(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute").ConstructorArguments.FirstOrDefault().Value as string));
                    }
                }
            }
            return datasetsByModel;
        }

        private static Dictionary<Type, GeospatialDatasetEndpoint> AddTimeSeriesEndpointsToGeospatialEndpointsByModel(Dictionary<Type, GeospatialDatasetEndpoint> datasetsByModel)
        {
            var timeSeriesControllers = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes())
               .Where(p => p.IsClass && p.BaseType != null && p.BaseType.IsGenericType && p.BaseType.GetGenericTypeDefinition() == (typeof(GeospatialTimeSeriesDatesetController<>)) && p.CustomAttributes.Any(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute" && att.ConstructorArguments.FirstOrDefault().ArgumentType == typeof(string))).ToList();

            //Find the underlying IChronometricable type associated to each TimeSeries controller, then find the associated ILocatable model for each using the AssociatedGeospatialModelAttribute
            foreach (var ctlr in timeSeriesControllers)
            {
                var timeSeriesType = ctlr.BaseType.GenericTypeArguments.FirstOrDefault(x => x.GetInterface("IChronometricable") != null);
                var geoType = timeSeriesType?.CustomAttributes?.FirstOrDefault(x=> x.AttributeType.Name == "AssociatedGeospatialModelAttribute" && x.ConstructorArguments.Any()).ConstructorArguments[0].Value as Type;
                if(geoType != null && datasetsByModel.ContainsKey(geoType))
                {
                    datasetsByModel[geoType].TimeSeriesEndpoints.Add(new TimeSeriesEndpoint(timeSeriesType.Name, ctlr.CustomAttributes.FirstOrDefault(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute").ConstructorArguments.FirstOrDefault().Value as string));
                }             
            }
            return datasetsByModel;
        }

        /*
        //Find all controllers extending GeospatialDatasetController, add name/endpoint pair for each
        var geospatialControllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && p.BaseType != null && p.BaseType.IsGenericType && p.BaseType.GetGenericTypeDefinition() == (typeof(GeospatialDatasetController<>)) && p.CustomAttributes.Any(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute" && att.ConstructorArguments.FirstOrDefault().ArgumentType == typeof(string))).ToList();
            if (geospatialControllers.Any())
            {
                datasetRoutes.Add("Geospatial", geospatialControllers.Select(t => new TimeseriesEndpoint(t.Name.Replace("Controller", ""), t.CustomAttributes.FirstOrDefault(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute").ConstructorArguments.FirstOrDefault().Value as string)).ToList());
            }


            //Find all controllers extending GeospatialTimeSeriesDatasetController, add name/endpoint pair for each
            var timeSeriesControllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && p.BaseType != null && p.BaseType.IsGenericType && p.BaseType.GetGenericTypeDefinition() ==(typeof(GeospatialTimeSeriesDatesetController<>)) && p.CustomAttributes.Any(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute" && att.ConstructorArguments.FirstOrDefault().ArgumentType == typeof(string) )).ToList();

            var timeseriesEndpoints = timeSeriesControllers.Select(t => t.BaseType).ToList();

            if (timeSeriesControllers.Any())
            {
                datasetRoutes.Add("TimeSeries", timeSeriesControllers.Select(t =>  new TimeseriesEndpoint(t.Name.Replace("Controller",""),t.CustomAttributes.FirstOrDefault(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute").ConstructorArguments.FirstOrDefault().Value as string)).ToList());
            }
            */


        public static List<GeospatialDatasetEndpoint> GetDatasetEndpoints()
        {
            return endpoints;
        }
    }
}