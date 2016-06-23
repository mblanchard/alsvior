using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers.GeospatialDatasets
{
    [RoutePrefix("api/weatherNode")]
    public class WeatherNodeController : GeospatialDatasetController<WeatherNode>
    {
        public WeatherNodeController(ICassandraClient client) : base(client) { }
    }
}
