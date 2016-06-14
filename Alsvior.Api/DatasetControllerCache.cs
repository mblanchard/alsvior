using Alsvior.Representations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsvior.Api
{
    public static class DatasetControllerCache
    {
        private static List<string> routes;

        public static void Init()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IGeospatialDatasetController).IsAssignableFrom(p) && !p.IsInterface && p.CustomAttributes.Any(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute" && att.ConstructorArguments.FirstOrDefault().ArgumentType == typeof(string) )).ToList();
            if (types.Any())
            {
                routes = types.Select(t => t.CustomAttributes.FirstOrDefault(att => att.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute").ConstructorArguments.FirstOrDefault().Value as string).ToList();
            }
        }

        public static List<string> GetDatasetEndpoints()
        {
            return routes;
        }
    }
}