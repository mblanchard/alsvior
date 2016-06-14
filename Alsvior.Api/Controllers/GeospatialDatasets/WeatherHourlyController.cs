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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/weatherHourly")]
    [Authorize]
    public class WeatherHourlyController : GeospatialTimeSeriesDatesetController<WeatherHourly>, IGeospatialDatasetController
    {
        public WeatherHourlyController(ICassandraClient client) : base(client) { }
    }

}
