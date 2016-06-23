using System.Collections.Generic;

namespace Alsvior.Representations.Models.DatasetEndpointDiscovery
{
    public class GeospatialDatasetEndpoint
    {
        public GeospatialDatasetEndpoint() { }
        public GeospatialDatasetEndpoint(string name, string uri) { Name = name; URI = uri; }


        public string Name { get; set; }     
        public string URI { get; set; }
        public List<TimeSeriesEndpoint> TimeSeriesEndpoints = new List<TimeSeriesEndpoint>();
    }
}
