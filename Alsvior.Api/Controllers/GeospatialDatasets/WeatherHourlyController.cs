using Alsvior.Core;
using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using Alsvior.Utility;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers.Geospatial
{
    [RoutePrefix("api/weatherHourly")]
    public class WeatherHourlyController : GeospatialTimeSeriesDatesetController<WeatherHourly>
    {
        public WeatherHourlyController(ICassandraClient client) : base(client) { }
    }

}
